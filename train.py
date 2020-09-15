# -*- coding: utf-8 -*-
"""
Created on Mon Sep 14 18:09:33 2020

@author: mistr
"""

import numpy as np
from grab_screen import grab_screen
import cv2
import time
from getkeys import key_check
import os
import keyboard


def keys_to_output():
    '''
    Convert keys to a ...multi-hot... array
    [A,W,D] boolean values.
    '''
    output = [0,0,0,0]
    
    if keyboard.read_key() == 'up':
        output[0] = 1
    elif keyboard.read_key() == 'down':
        output[1] = 1
    elif keyboard.read_key() == 'left':
        output[2] = 1
    elif keyboard.read_key()== 'right':
        output[3] = 1
    return output


file_name = 'D:/CSCI 527/training_data/training_data.npy'



if os.path.isfile(file_name):
    print('File exists, moving along')
    training_data = list(np.load(file_name,allow_pickle=True)) 
else:
    print('File does not exist, starting fresh!')
    training_data = []


def main():

    for i in list(range(4))[::-1]:
        print(i+1)
        time.sleep(1)


    paused = False
    while(True):

        if not paused:
            # 800x600 windowed mode
            print('I am taking SS')
            screen = grab_screen(region=(0,200,800,800))
            last_time = time.time()
            screen = cv2.cvtColor(screen, cv2.COLOR_BGR2GRAY)
            screen = cv2.resize(screen, (160,120))
            # resize to something a bit more acceptable for a CNN
            #keys = key_check()
            output = keys_to_output()
            print(output)
            training_data.append([screen,output])
            print(len(training_data))
            
            if len(training_data) % 1500 == 0:
                print(len(training_data))
                np.save(file_name,training_data)

        # keys = key_check()
        # if 'T' in keys:
        #     if paused:
        #         paused = False
        #         print('unpaused!')
        #         time.sleep(1)
        #     else:
        #         print('Pausing!')
        #         paused = True
        #         time.sleep(1)


main()