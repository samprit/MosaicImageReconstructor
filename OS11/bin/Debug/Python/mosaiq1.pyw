from os import listdir
from os.path import isfile, join
import sys
import cv2
import numpy as np
from numpy import array
import math
import copy
from Image import Image
import ctypes.wintypes
from collections import defaultdict

# fptr=open("config.json")
# params=json.load(fptr)

####################
# Global Variables 
#polynomialApproximation=0
variancePercentage = 0.5
meanPercentage = 0.5
arcLengthmultiplier = 10
minVarianceThreshold = 0 # 1% of the max images height, width
noiseReduction = 1  # 0 means false 1 true
scaleFactor =1000
meanDiffThreshold =0
maxVariance = 0
approxPolyparameter = 0.07
#####################

def getAbsolutePaths(directoryPath):
	onlyfiles = [ f for f in listdir(directoryPath) if isfile(join(directoryPath,f)) ]
	filenames = []
	for image in onlyfiles:
		filenames.append(directoryPath+image)
	return filenames

def getAllFilePaths(inputTextFilePath):
	filePaths = [line.strip() for line in open(inputTextFilePath)]
	#print filePaths
	return filePaths

def getMeansVariances(edgeEnvelopes):
	means = []
	variances = []
	i =0 
	for edgeEnvelope in edgeEnvelopes:
		mean = sum(edgeEnvelope)/len(edgeEnvelope)
		means.append(mean)

		variance = 0
		for dist in edgeEnvelope:
			variance+=(dist-mean)**2

		variance/=len(edgeEnvelope)
		variances.append(variance)
		
	return means, variances

def getEdgeEnvelopes(contoursList, edges):
	edgeEnvelopes = []
	lenContoursList = len(contoursList)
	
	for i in range(len(edges)):
		start = edges[i][0]
		end = edges[i][1]

		if end < start:
			end = end+lenContoursList
		
		inrangeEnd = end%lenContoursList
		B = np.array([contoursList[start][0],contoursList[start][1]])
		C = np.array([contoursList[inrangeEnd][0],contoursList[inrangeEnd][1]])
		tempList = []
		
		for j in range(start, end):
			inrangeJ = j%lenContoursList
			A = np.array([contoursList[inrangeJ][0],contoursList[inrangeJ][1]])

			angle1 = math.atan2(contoursList[inrangeEnd][1]-contoursList[start][1], contoursList[inrangeEnd][0]-contoursList[start][0])
			angle2 = math.atan2(contoursList[inrangeEnd][1]-contoursList[inrangeJ][1], contoursList[inrangeEnd][0]-contoursList[inrangeJ][0])
			angle = angle1 - angle2

			isleft = ( ((B[0] - C[0])*(A[1] - C[1])) - ((B[1] - C[1])*(A[0] - C[0])) ) > 0
			perpendicularDist = math.sqrt((contoursList[inrangeEnd][1]-contoursList[inrangeJ][1])**2 + (contoursList[inrangeEnd][0]-contoursList[inrangeJ][0])**2)
			perpendicularDist*=math.fabs(math.sin(angle))
			
			if(isleft):
				perpendicularDist *=-1
			tempList.append(perpendicularDist)
		
		edgeEnvelopes.append(tempList)
	return edgeEnvelopes

def isStraightEdge1(variance):
	global minVarianceThreshold
	if(variance > minVarianceThreshold):
		return 0
	return 1
	
def getNEED(edge1, edge2):
	len1 = len(edge1)
	len2 = len(edge2)

	M = min(len1, len2)
	need=0
	if(len1==M):
		for i in range(M):
			need+=math.fabs(edge1[i]+edge2[int(float(i)*float(len2)/len1)])
	else:
		for i in range(M):
			need+=math.fabs(edge2[i]+edge1[int(float(i)*float(len1)/len2)])

	need/=M
	return need

def processImage(im,filename):
	original = np.copy(im)
	original1 = np.copy(im)

	blur = cv2.GaussianBlur(im, (3,3), 3, 3)
	imgray = cv2.cvtColor(blur, cv2.COLOR_RGB2GRAY)
		
	histogram = defaultdict()
	histogram = defaultdict(lambda: 0, histogram)
		
	imgrayList = imgray.tolist()
	
	for rowIndex in range(4):
		for pixel in imgrayList[rowIndex]:
			histogram[pixel]+=1
			
	for rowIndex in range(len(imgrayList)-4, len(imgrayList)):
		for pixel in imgrayList[rowIndex]:
			histogram[pixel]+=1
			
	for columnIndex in range(4):
		for i in range (4,len(imgrayList)-4):
			histogram[imgrayList[i][columnIndex]]+=1
	
	for columnIndex in range(len(imgrayList[0])-4,len(imgrayList[0])):
		for i in range (4,len(imgrayList)-4):
			histogram[imgrayList[i][columnIndex]]+=1
	
	maxval = -1
	maxi = None
	for i in range(256):
		#print i, histogram[i]
		if histogram[i] > maxval:
			maxval = histogram[i]
			maxi = i

	thresh = None
	ret = None
	binary = None
	if maxi>127:
		thresh = maxi-2
		ret, binary = cv2.threshold(imgray, thresh, 255, 1)
	else:
		thresh = maxi+3
		ret, binary = cv2.threshold(imgray, thresh, 255, 0)
		
	global noiseReduction
	if(noiseReduction):
		kernel = np.ones((4,4),np.uint8)
		dest = cv2.morphologyEx(binary, 3, kernel, iterations = 1)
		kernel = np.ones((5,5),np.uint8)
		dest = cv2.erode(dest, kernel, iterations = 2)
	else:
		dest = binary
	
	contours, hierarchy = cv2.findContours(dest, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
	
	'''Find contour of max area in this image'''
	cnt = None
	maxArea = -1
	for contour in contours:
		area = cv2.contourArea(contour)
		if(area > maxArea):
			cnt = contour
			maxArea = area
	
	cv2.drawContours(original1, cnt, -1, (0,0,255), 2)
	
	global approxPolyparameter
	approx = cv2.approxPolyDP(cnt,approxPolyparameter*cv2.arcLength(cnt,True),True)
	
	#Stored in anticlockwise order
	cv2.drawContours(original1, approx, -1, (0,0,255), 30)
	
	'''Remove need for extra referencing'''
	contoursList = []
	for point in cnt:
		contoursList.append(point[0])
	
	'''Remove need for extra referencing'''
	boundary = []
	for point in approx:
		boundary.append(point[0])
	
	'''Find indices of corner points in contoursList'''
	lenBoundary = len(boundary)
	indices = []
	j = 0
	for i in range(2*len(contoursList)):
		if boundary[j][0]==contoursList[i%len(contoursList)][0] and boundary[j][1]==contoursList[i%len(contoursList)][1]:
			indices.append(i%len(contoursList))
			j+=1
			if j==lenBoundary:
				break
				
	edgesindex = []				
	for i in range(lenBoundary):
		k = (indices[i],indices[(i+1)%lenBoundary]) 
		edgesindex.append(k)
	
	#### Uncomment for 2 edges at once	
	#for i in range(lenBoundary):
	#	k = (indices[i],indices[(i+2)%lenBoundary] 
	#	edges.append(k)
	
	edgeEnvelopes = getEdgeEnvelopes(contoursList, edgesindex)
	
	means,variances = getMeansVariances(edgeEnvelopes)
	
	global maxVariance
	maxVariance = max(maxVariance,max(variances))
	image1 = Image(original, cnt, contoursList, boundary, edgesindex, edgeEnvelopes, means, variances, None)
	return image1

def preProcess(filenames):	
	images = []
	global minVarianceThreshold
	global meanDiffThreshold
	global variancePercentage
	global meanPercentage
	global scaleFactor	
	k=0	
	scale =0;
	maxWidth =0
	for filename in filenames:
		im = cv2.imread(filename, -1)
		if(k==0):
			scale = int(1 + min(im.shape[0],im.shape[1])/scaleFactor)
			k=1
		result = cv2.resize(im, (im.shape[1]/scale,im.shape[0]/scale))
		
		maxWidth = max(maxWidth,result.shape[0],result.shape[1])
		images.append(processImage(result,filename))	
			
	minVarianceThreshold = int((variancePercentage/100)*maxWidth);
	meanDiffThreshold = int((meanPercentage/100)*maxWidth);
	return images

def mergeImages(image1, image2, edge1, edge2):
	original1 = image1.original
	original2 = image2.original
	
	newimagex =2 * max(original1.shape[0],original2.shape[0])
	newimagey =2 * max(original1.shape[1],original2.shape[1])
	
	contour1 = image1.contour
	contour2 = image2.contour
	
	contoursList1 = image1.contoursList
	contoursList2 = image2.contoursList

	edgesindex1 = image1.edgesindex
	edgesindex2 = image2.edgesindex

	start1 = contoursList1[edgesindex1[edge1][0]]; 
	end1 = contoursList1[edgesindex1[edge1][1]]
	
	start2 = contoursList2[edgesindex2[edge2][1]]; 
	end2 = contoursList2[edgesindex2[edge2][0]]

	shiftx1 = int(newimagex/2) - start1[0]
	shifty1 = int(newimagey/2) - start1[1]
	start1 = [start1[0] + shiftx1,start1[1] + shifty1]
	end1 = [end1[0] + shiftx1 , end1[1] + shifty1]
	
	shiftx2 = int(newimagex/2) - start2[0]
	shifty2 = int(newimagey/2) - start2[1]
	start2 = [start2[0] + shiftx2,start2[1] + shifty2]
	end2 = [end2[0] + shiftx2 , end2[1] + shifty2]

				
	angle1 = math.atan2(end1[1] - start1[1], end1[0] - start1[0])
	angle2 = math.atan2(end2[1] - start2[1], end2[0] - start2[0])
	angle = angle2 - angle1
	r = math.sqrt((end2[1]-start2[1])**2+(end2[0]-start2[0])**2)
	
	newStart2 = start2
	newEnd2 = [start2[0]+r*math.cos(angle1), start2[1]+r*math.sin(angle1)]
	

	tx = 0.5*((end1[0]+start1[0] )-(newEnd2[0]-newStart2[0]))
	ty = 0.5*((end1[1]+start1[1] )-(newEnd2[1]-newStart2[1]))

	img1 = image1.original
	img2 = image2.original

	mask1 = np.zeros((newimagey, newimagex,3),np.uint8)
	cv2.fillPoly(mask1, pts =[image1.contour], color=(1,1,1))
	mask1[:img1.shape[0], :img1.shape[1]] = np.multiply(img1,mask1[:img1.shape[0],:img1.shape[1]]);
	
	rows1,cols1,depth = mask1.shape

	M = np.float32([[1,0,shiftx1],[0,1,shifty1]])
	mask1 = cv2.warpAffine(mask1,M,(cols1,rows1))
	
	mask2 = np.zeros((newimagey, newimagex,3),np.uint8)
	cv2.fillPoly(mask2, pts =[image2.contour], color=(1,1,1))
	mask2[:img2.shape[0], :img2.shape[1]] = np.multiply(img2,mask2[:img2.shape[0],:img2.shape[1]]);
	
	rows2,cols2,depth = mask2.shape

	M = np.float32([[1,0,shiftx2],[0,1,shifty2]])
	mask2 = cv2.warpAffine(mask2,M,(cols2,rows2))

	M = cv2.getRotationMatrix2D((int(newimagex/2), int(newimagey/2)),angle*180/math.pi,1)
	mask2 = cv2.warpAffine(mask2,M,(newimagex, newimagey))
	
	M = np.float32([[1,0,tx-newStart2[0]],[0,1,ty-newStart2[1]]])
	mask2 = cv2.warpAffine(mask2,M,(newimagex, newimagey))

	mask1 = mask1 + mask2
	
	return mask1

def matchImages(images,straights):
	
	globalMinNeed = 100000000000.0
	part1 = None
	part2 = None
	edge1 = None
	edge2 = None
	
	for imagei in range(len(images)):
		for imagej in range(len(images)):
			if imagej<=imagei:#No repetition
				continue

			edgesindexi = images[imagei].edgesindex
			edgesindexj = images[imagej].edgesindex
			
			contoursListi = images[imagei].contoursList
			contoursListj = images[imagej].contoursList
			
			edgeEnvelopesi = images[imagei].edgeEnvelopes
			edgeEnvelopesj = images[imagej].edgeEnvelopes
		
			straighti = straights[imagei]
			straightj = straights[imagej]

			minNEED = 1000000000.0
			index1 = None
			index2 = None
			
			for i in range(len(edgeEnvelopesi)):
				if straighti[i]:
					continue
				for j in range(len(edgeEnvelopesj)):
					if straightj[j]:							
						continue
					
					#Use pixel count of contour, better metric
					pixelCount1 = len(edgeEnvelopesi[i])
					pixelCount2 = len(edgeEnvelopesj[j])
					if math.fabs(pixelCount1 - pixelCount2) > 0.2*min(pixelCount1, pixelCount2):
						continue

					#Distance not used currently
					start = contoursListi[edgesindexi[i][0]]
					end = contoursListi[edgesindexi[i][1]]		
					dist1 = math.sqrt((start[0]-end[0])**2 + (start[1]-end[1])**2)
					
					start = contoursListj[edgesindexj[j][0]]
					end = contoursListj[edgesindexj[j][1]]		
					dist2 = math.sqrt((start[0]-end[0])**2 + (start[1]-end[1])**2)
					
					if( math.fabs(dist1-dist2) > 0.1*min(dist1,dist2)):
						continue

					arcStart1 = edgesindexi[i][0]
					arcEnd1 = edgesindexi[i][1]
					
					if arcEnd1>arcStart1:
						arcList1 = images[imagei].contour[arcStart1:arcEnd1]
					else:
						arcList1 = images[imagei].contour[arcStart1:]
						np.append(arcList1, images[imagei].contour[:arcEnd1])
														
					arcLength1 = cv2.arcLength(arcList1, False)
					
					arcStart2 = edgesindexj[j][0]
					arcEnd2 = edgesindexj[j][1]
					
					if arcEnd2>arcStart2:
						arcList2 = images[imagej].contour[arcStart2:arcEnd2]
					else:
						arcList2 = images[imagej].contour[arcStart2:]
						np.append(arcList2, images[imagej].contour[:arcEnd2])
												
					arcLength2 = cv2.arcLength(arcList2, False)
					
					NEED = getNEED(edgeEnvelopesi[i], edgeEnvelopesj[j])
								
					minVariance = min(images[imagei].variances[i],images[imagej].variances[j])					
					meanDiff = math.fabs(images[imagei].means[i]+images[imagej].means[j])
					
					global arcLengthmultiplier
					global minVarianceThreshold
					global meanDiffThreshold
					global maxVariance
					
					if(math.fabs(meanDiff) > meanDiffThreshold):
						continue

					NEEDED = NEED*(1+arcLengthmultiplier*(math.fabs(arcLength1-arcLength2)/min(arcLength1, arcLength2)))
					#NEEDED = NEED*(1+arcLengthmultiplier*(math.fabs(dist1-dist2)/min(dist1, dist2)))
					
					NEEDED = NEEDED/(1+ 3*math.sqrt(minVariance/maxVariance))
				
					if NEEDED < minNEED:
						minNEED = NEEDED
						index1 = i
						index2 = j

			if minNEED < globalMinNeed:
				globalMinNeed = minNEED
				part1 = imagei
				part2 = imagej
				edge1 = index1
				edge2 = index2
				
	return part1,part2,edge1,edge2
	
def writeOutput(image):
	directoryPath = sys.argv[2]
	cv2.imwrite(directoryPath+'Final.jpg', image.original)
	CSIDL_PERSONAL= 5       # My Documents
	SHGFP_TYPE_CURRENT= 0   # Want current, not default value
	buf= ctypes.create_unicode_buffer(ctypes.wintypes.MAX_PATH)
	ctypes.windll.shell32.SHGetFolderPathW(0, CSIDL_PERSONAL, 0, SHGFP_TYPE_CURRENT, buf)
	finishPath = buf.value + "\\MosaiQ\\finish.txt"
	f = open(finishPath,'wb')
	f.write("done")
	f.close()
	exit()

def main():
	if(len(sys.argv)!=3):
		print "Invalid input format!!"
		exit()
	
	#filenames = getAbsolutePaths(sys.argv[1])
	filenames = getAllFilePaths(sys.argv[1])
	images = preProcess(filenames)

	while len(images)>1:
		straights = []
		for image in images:
			edgeEnvelopes = image.edgeEnvelopes
			straight = []
			for i in range(len(edgeEnvelopes)):
				tmp = isStraightEdge1(image.variances[i])
				straight.append(tmp)
			straights.append(straight)
						
		part1,part2,edge1,edge2 = matchImages(images,straights)

		# processing the merged image 
		if(edge1!=None):
			mask = mergeImages(images[part1], images[part2], edge1, edge2)
			mergeimage = processImage(mask, "merged Image")
		else:
			writeOutput(images[0])
			
		#removing previous images
		if(part2>part1):
			del images[part2]
			del images[part1]
		else:
			del images[part1]
			del images[part2]

		#trimming the images 	 
		cnt = mergeimage.contour
		x,y,w,h = cv2.boundingRect(cnt)
		trimmed = mask[max(y-h/5,0):min(y+h+h/5, mask.shape[1]), max(x-w/5, 0):min(x+w+w/5, mask.shape[0])]
		
		#processing new trimmed image and storing in images
		newTrimmed = processImage(trimmed,"trimmed Image")
		images.append(newTrimmed);
	writeOutput(images[0])
if __name__ == "__main__":
	main()
