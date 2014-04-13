import cv2
import numpy as np
import math
import copy

'''Here boundary contains all the points of this contour in anti-clockwise order
angles contains all angles, prevLengths contains all previous lengths,
nextLengths contains all next lengths'''
class Image:
	original = None
	contour = None
	contoursList = None
	boundary = None
	indices = None
	edgeEnvelopes = None
	angles = None
	prevLengths = None
	nextLengths = None

	def __init__(self, original = None, contour = None, contoursList = None, boundary = None, indices = None, edgeEnvelopes = None,angles = None, prevLengths = None, nextLengths = None):
		self.original = original
		self.contour = contour
		self.contoursList = contoursList
		self.boundary = boundary
		self.indices = indices
		self.edgeEnvelopes = edgeEnvelopes
		self.angles = angles
		self.prevLengths = prevLengths
		self.nextLengths = nextLengths
