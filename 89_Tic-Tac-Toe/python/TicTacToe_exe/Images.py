import os
import game_config as gc

from pygame import image, transform

def GetName(val):
    return str(val) + ".png"

class Image: 
    def __init__(self, val):
        self.name = GetName(val) #names of image with .png 
        self.image_path = os.path.join(gc.ASSET_DIR, self.name) #path of image from the assets file
        self.image = image.load(self.image_path) #loaded the image
        self.image = transform.scale(self.image, (gc.IMAGE_SIZE - 2 * gc.MARGIN, gc.IMAGE_SIZE - 2 * gc.MARGIN)) #fixed image as per req.
       # self.box = self.image.copy() # box
        #self.box.fill((200, 200, 200))



if __name__ == '__main__':
    for i in gc.ASSET_FILES:
        temp = Image(i)
        print(temp.name,temp.image_path,temp.image,temp)
