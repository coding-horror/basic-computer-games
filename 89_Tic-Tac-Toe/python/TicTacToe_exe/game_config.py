import os

IMAGE_SIZE = 103
SCREEN_SIZE = 321
NUM_TILES_SIDE = 3
NUM_TILES_TOTAL = 9
MARGIN = 2

ASSET_DIR = 'assets'
ASSET_FILES = [x for x in os.listdir(ASSET_DIR)]
#assert len(ASSET_FILES) == 8