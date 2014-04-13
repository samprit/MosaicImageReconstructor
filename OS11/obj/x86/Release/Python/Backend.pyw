from os import listdir
from os.path import isfile, join
import sys
import cv2
import numpy as np
from numpy import array
import math
import copy
import ctypes.wintypes
from Image import Image
from collections import defaultdict

def getAbsolutePaths(directoryPath):
	
	onlyfiles = [ f for f in listdir(directoryPath) if isfile(join(directoryPath,f)) ]
	filenames = []
	for image in onlyfiles:
		filenames.append(directoryPath+image)

	#print filenames
	return filenames

def getAllFilePaths(inputTextFilePath):
	filePaths = [line.strip() for line in open(inputTextFilePath)]
	#print filePaths
	return filePaths

def displayImage(windowName, image):
	cv2.namedWindow(windowName, cv2.WINDOW_NORMAL)
	cv2.resizeWindow(windowName, 600, 400)
	cv2.imshow(windowName, image)

'''Get envelopes for all edges for an image'''
def getEdgeEnvelopes(contoursList, indices):
	edgeEnvelopes = []
	lenContoursList = len(contoursList)
	for i in range(len(indices)):
		start = indices[i]
		end = indices[(i+1)%len(indices)]

		if end < start:
			end = end+lenContoursList

		tempList = []
		for j in range(start, end):
			inrangeEnd = end%lenContoursList
			inrangeJ = j%lenContoursList
			angle1 = math.atan2(contoursList[inrangeEnd][1]-contoursList[start][1], contoursList[inrangeEnd][0]-contoursList[start][0])
			angle2 = math.atan2(contoursList[inrangeEnd][1]-contoursList[inrangeJ][1], contoursList[inrangeEnd][0]-contoursList[inrangeJ][0])
			angle = angle1 - angle2

			perpendicularDist = math.sqrt((contoursList[inrangeEnd][1]-contoursList[inrangeJ][1])**2 + (contoursList[inrangeEnd][0]-contoursList[inrangeJ][0])**2)
			perpendicularDist*=math.sin(angle)
			tempList.append(perpendicularDist)

		edgeEnvelopes.append(tempList)

	return edgeEnvelopes
	
def isStraightEdge(edge):
	for dist in edge:
		
		if math.fabs(dist) > 50:
			return 0
	return 1
	
'''Returns minimum NEED between 2 edges and the corresponding direction'''
'''How to interpolate, for now use min'''
def getNEED(edge1, edge2):
	len1 = len(edge1)
	len2 = len(edge2)

	M = min(len1, len2)
	need=0
	if(len1==M):
		for i in range(M):
			need+=math.fabs(edge1[i]-edge2[int(float(i)*float(len2)/len1)])
	else:
		for i in range(M):
			need+=math.fabs(edge2[i]-edge1[int(float(i)*float(len1)/len2)])

	need/=M
	return need,1
		
'''Reads all the input images and extracts the features which are returned in images'''

def processImage(im,filename):
	original = np.copy(im)	
	blur = cv2.GaussianBlur(im, (3,3), 3, 3)
	#print blur
	imgray = cv2.cvtColor(blur, cv2.COLOR_BGR2GRAY)
	
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
		if histogram[i] > maxval:
			maxval = histogram[i]
			maxi = i
			
	thresh = None
	ret = None
	binary = None
	if maxi>127:
		thresh = maxi-3
		ret, binary = cv2.threshold(imgray, thresh, 255, 1)
	else:
		thresh = maxi+3
		ret, binary = cv2.threshold(imgray, thresh, 255, 0)
	
	kernel = np.ones((4,4),np.uint8)
	dest = cv2.morphologyEx(binary, 3, kernel, iterations = 1)
	kernel = np.ones((5,5),np.uint8)
	dest = cv2.erode(dest, kernel, iterations = 2)
	contours, hierarchy = cv2.findContours(dest, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
	
	'''Find contour of max area in this image'''
	cnt = None
	maxArea = -1
	for contour in contours:
		area = cv2.contourArea(contour)
		if(area > maxArea):
			cnt = contour
			maxArea = area
	
	approx = cv2.approxPolyDP(cnt,0.05*cv2.arcLength(cnt,True),True)
	#Stored in anticlockwise order
	
	'''Remove need for extra referencing'''
	contoursList = []
	for point in cnt:
		contoursList.append(point[0])
	
	'''Remove need for extra referencing'''
	boundary = []
	for point in approx:
		boundary.append(point[0])
	
	'''Find indices of corner points in contoursList'''
	indices = []
	j = 0
	for i in range(2*len(contoursList)):
		if boundary[j][0]==contoursList[i%len(contoursList)][0] and boundary[j][1]==contoursList[i%len(contoursList)][1]:
			indices.append(i%len(contoursList))
			j+=1
			if j==len(boundary):
				break

	edgeEnvelopes = getEdgeEnvelopes(contoursList, indices)
	image1 = Image(original, cnt, contoursList, boundary, indices, edgeEnvelopes, None, None, None)
	return image1
	
def preProcess(filenames):
	images = []

	for filename in filenames:
		im = cv2.imread(filename, -1)		
		images.append(processImage(im,filename))
	return images

def mergeImages(image1, image2, edge1, edge2):
	original1 = image1.original
	original2 = image2.original
	
	newimagex =2 * max(original1.shape[0],original2.shape[0])
	newimagey =2 * max(original1.shape[1],original2.shape[1])
	
	contour1 = image1.contour
	contour2 = image2.contour

	boundary1 = image1.boundary
	boundary2 = image2.boundary

	start1 = boundary1[edge1];end1 = boundary1[(edge1+1)%len(boundary1)]
	start2 = boundary2[(edge2+1)%len(boundary2)];end2 = boundary2[edge2]

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
	direction = None
	
	for imagei in range(len(images)):
		for imagej in range(len(images)):
			if imagej<=imagei:#No repetition
				continue

			boundaryi = images[imagei].boundary
			boundaryj = images[imagej].boundary

			edgeEnvelopesi = images[imagei].edgeEnvelopes
			edgeEnvelopesj = images[imagej].edgeEnvelopes
			
			straighti = straights[imagei]
			straightj = straights[imagej]

			minNEED = 1000000000.0
			index1 = None
			index2 = None
			minDirection = None
			for i in range(len(edgeEnvelopesi)):
				if straighti[i]:
					continue
				for j in range(len(edgeEnvelopesj)):
					if straightj[j]:
						continue
					
					#Use pixel count of contour, better metric
					pixelCount1 = len(edgeEnvelopesi[i])
					pixelCount2 = len(edgeEnvelopesj[j])
					if math.fabs(pixelCount1 - pixelCount2) > 0.4*max(pixelCount1, pixelCount2):
						continue

					#Distance not used currently
					start = boundaryi[i]
					end = boundaryi[(i+1)%len(boundaryi)]		
					dist1 = math.sqrt((start[0]-end[0])**2 + (start[1]-end[1])**2)
					
					indices1 = images[imagei].indices
					arcStart1 = indices1[i]
					arcEnd1 = indices1[(i+1)%len(indices1)]
					
					if arcEnd1>arcStart1:
						arcList1 = images[imagei].contour[arcStart1:arcEnd1]
					else:
						arcList1 = images[imagei].contour[arcStart1:]
						np.append(arcList1, images[imagei].contour[:arcEnd1])
													
					arcLength1 = cv2.arcLength(arcList1, False)
					indices2 = images[imagej].indices
					arcStart2 = indices2[j]
					arcEnd2 = indices2[(j+1)%len(indices2)]
					
					if arcEnd2>arcStart2:
						arcList2 = images[imagej].contour[arcStart2:arcEnd2]
					else:
						arcList2 = images[imagej].contour[arcStart2:]
						np.append(arcList2, images[imagej].contour[:arcEnd2])
							
					arcLength2 = cv2.arcLength(arcList2, False)
					
					start = boundaryj[j]
					end = boundaryj[(j+1)%len(boundaryj)]		
					dist2 = math.sqrt((start[0]-end[0])**2 + (start[1]-end[1])**2)
					if math.fabs(dist1-dist2) > 0.4*max(dist1, dist2):
						continue

					NEED, direction = getNEED(edgeEnvelopesi[i], edgeEnvelopesj[j])
					NEEDED = NEED
					NEEDED = NEED*(1+10*(math.fabs(arcLength1-arcLength2)/min(arcLength1, arcLength2)))
					if NEEDED < minNEED:
						minNEED = NEEDED
						index1 = i
						index2 = j
						minDirection = direction

			#Have to ignore flat edges. Set very high NEED value

			if minNEED < globalMinNeed:
				globalMinNeed = minNEED
				part1 = imagei
				part2 = imagej
				edge1 = index1
				edge2 = index2
				direction = minDirection
				
	return part1,part2,edge1,edge2
	
def writeOutput(image):
	outputDirectory = sys.argv[2]
	cv2.imwrite(outputDirectory+'Final.png', image.original)
	#print "done"
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
		#print "Invalid input format!!"
		exit()	

	#filenames = getAbsolutePaths(sys.argv[1])
	filenames = getAllFilePaths(sys.argv[1])
	images = preProcess(filenames)
	while len(images)>1:
		straights = []
		for image in images:		
			edgeEnvelopes = image.edgeEnvelopes
			straight = []
		
			for edge in edgeEnvelopes:
				straight.append(isStraightEdge(edge))
			
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