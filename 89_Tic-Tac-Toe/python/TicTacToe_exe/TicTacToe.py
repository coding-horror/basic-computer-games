from ctypes.wintypes import PINT
from pickle import TRUE
from numpy import flip, source
import pygame
import game_config as gc
from process import TicTacToe as T
from pygame import display, event, image
from time import sleep
import Images

def initial() -> pygame.Surface:
    pygame.init()
    display.set_caption('Tic-Tac-Toe')
    screen = display.set_mode((gc.SCREEN_SIZE, gc.SCREEN_SIZE))
    print( type(screen) )
    return screen

def find_xy(x, y):
    row = y // gc.IMAGE_SIZE
    col = x // gc.IMAGE_SIZE
    return row, col

def update_board_display(screen : pygame.Surface, Game : T ):#Update only
    screen.blit(image.load('assets/blank.png'), (0, 0))
    #sleep(1)
    screen.fill((0, 0, 0))
    for i in range(gc.NUM_TILES_SIDE):
        for j in range(gc.NUM_TILES_SIDE):
            tile = Images.Image(Game.board[i][j])
            screen.blit(tile.image, (j * gc.IMAGE_SIZE + gc.MARGIN, i * gc.IMAGE_SIZE + gc.MARGIN))
    display.flip()
    #sleep(1)

def GameOver(Game : T,screen : pygame.Surface) -> bool:
    val = Game.CheckWin()
    if val == 1:
        display.flip()
        update_board_display(screen,Game=Game)
        sleep(1.5)
        screen.blit(image.load('assets/win.png'), (0, 0))
        display.flip()
        sleep(2.3)
        return True
    if val == 0:
        display.flip()
        update_board_display(screen,Game=Game)
        sleep(1.5)
        screen.blit(image.load('assets/lose.png'), (0, 0))
        display.flip()
        sleep(2.3)
        return TRUE
    return False


def run(Game : T,screen : pygame.Surface, running : bool):
    update_board_display(screen,Game)
    display.flip()
    while running:
        current_events = event.get()
        for e in current_events:
            if e.type == pygame.QUIT:# clicked X
                running = False

            if e.type == pygame.KEYDOWN: #keyboard
                if e.key == pygame.K_ESCAPE:# Esc to end it
                    running = False

            if e.type == pygame.MOUSEBUTTONDOWN: #clickd
                mouse_x, mouse_y = pygame.mouse.get_pos()# got position
                row, col = find_xy(mouse_x, mouse_y)#location of click
                if row >= gc.NUM_TILES_SIDE or col >= gc.NUM_TILES_SIDE:# if it's on screen 
                    continue
                if Game.MoveRecord(row,col) == True:#if the move is possible
                    if GameOver(Game,screen):#if game ends with it
                        running = False
                        break
                    #game is still on
                    a,b = Game.NextMove()# computer makes the move
                    print(a,b, " here ")
                    if a == -1 and b == -1: #special condition for tie
                        running = False #end the game
                        screen.blit(image.load('assets/tie.png'), (0, 0))
                        display.flip()
                        sleep(2.3)
                        break;
                    else:#not a tie
                        if GameOver(Game,screen):#if game ends with it
                            running = False
                            break
                    
                    update_board_display(screen=screen,Game=Game)
                display.flip()
                sleep(2.1)

    #screen.blit(tile.image, (j * gc.IMAGE_SIZE + gc.MARGIN, i * gc.IMAGE_SIZE + gc.MARGIN))

def X_or_O(screen : pygame.Surface) -> str:
    X = Images.Image("x")
    O = Images.Image("o")
    #pygame.transform.scale()
    # Surface, (width, height) -> Surface
    X.image = pygame.transform.scale(X.image,(gc.SCREEN_SIZE//2,gc.SCREEN_SIZE))
    O.image = pygame.transform.scale(O.image,(gc.SCREEN_SIZE//2,gc.SCREEN_SIZE))
    screen.blit(X.image, (gc.MARGIN, gc.MARGIN))
    screen.blit(O.image, (gc.MARGIN +gc.SCREEN_SIZE//2 , gc.MARGIN))
    #screen.blits( blit_sequence=[ (X.image , (0,0) , pygame.Rect(0,0,gc.SCREEN_SIZE//2,gc.SCREEN_SIZE//2)), (O.image , (gc.SCREEN_SIZE//2,gc.SCREEN_SIZE//2) , pygame.Rect(gc.SCREEN_SIZE//2,gc.SCREEN_SIZE//2,gc.SCREEN_SIZE,gc.SCREEN_SIZE)) ] )
    display.flip()
    pick = "-1"
    while True:
        current_events = event.get()
        for e in current_events:
            if e.type == pygame.QUIT:# clicked X
                return pick

            if e.type == pygame.KEYDOWN: #keyboard
                if e.key == pygame.K_ESCAPE:# Esc to end it
                    return pick
            if e.type == pygame.MOUSEBUTTONDOWN:
                x,y = pygame.mouse.get_pos()
                if (x < gc.SCREEN_SIZE and x >= 0) or (y < gc.SCREEN_SIZE and y > 0):# if it's on screen 
                    if x > gc.SCREEN_SIZE//2:
                        pick = "O"
                    else: pick = "X"
                screen.fill((0,0,0))
                display.flip()
                sleep(1.1)
                return pick
    return pick


def play():
    screen = initial()
    running = True
    pick = X_or_O(screen=screen)
    if pick == "-1":
        return
    Game = T(pick,gc.NUM_TILES_SIDE)
    run(Game,screen,running)

if __name__ == "__main__":

    print("Hello World!")
    play()
    print('Goodbye!')

