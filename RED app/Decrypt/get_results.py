#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Wed Feb 21 12:03:21 2018

@author: Alex Irvine. airvine1991@gmail.com 

"""
#from rijndael.cipher.crypt import new
#from rijndael.cipher.blockcipher import MODE_CBC
#import pprp
#import pprp.config

from firebase import firebase as fb; 
import base64; 
import os
import sys 


#Change current directory to that of this file


#abspath = os.path.abspath(__file__)
#dname = os.path.dirname(abspath)
#os.chdir(dname)

# %%
#Get the big clump of JSON 
auth = fb.FirebaseAuthentication('Cs6Su1Up797B2LWD7QwrSWF5y7tWtwpr7rbyv0IL', 'main')
database = fb.FirebaseApplication('https://redapp-alpha.firebaseio.com/', authentication= auth);
result = database.get('', None);


#loop through entries
for key in result:
    
    #filename 
    filename = result[key]['filename'];
    
    #string 
    string64 = result[key]['file_string'];
    
    #decode 
    string_decoded = base64.b64decode(string64); 
    
    text_file = open(filename, "w")
    text_file.write(string_decoded.decode("utf-8"))
    text_file.close()
    
