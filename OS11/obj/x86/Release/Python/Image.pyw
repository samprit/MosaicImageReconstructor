import cv2
import numpy as np

'''Here boundary contains all the points of this contour in anti-clockwise order
angles contains all angles, prevLengths contains all previous lengths,
nextLengths contains all next lengths'''
class Image:
	original = None
	contour = None
	contoursList = None
	boundary = None
	edgesindex = None
	edgeEnvelopes = None
	means = []
	variances = []
	nextLengths = None

	def __init__(self, original = None, contour = None, contoursList = None, boundary = None, edgesindex = None, edgeEnvelopes = None,means = None, variances = None, nextLengths = None):
		self.original = original
		self.contour = contour
		self.contoursList = contoursList
		self.boundary = boundary
		self.edgesindex = edgesindex
		self.edgeEnvelopes = edgeEnvelopes
		self.means = means
		self.variances = variances
		self.nextLengths = nextLengths
