import os
import pefile
import re
import psutil
import ctypes
import tkinter as tk
from tkinter import *
from tkinter import ttk
import sv_ttk
from mem_edit_ss import Process
import configparser
import math
# from win32api import GetFileVersionInfo, LOWORD, HIWORD
'''
======================================================= Global Variables =======================================================
'''
ui_default_size = '600x390'

GameVersion = "0"
GameDefaultNameplateRange = 400.00
GameDefaultFOV = 1.5707963267948966
GameDefaultCameraDistance = 50.00
GameDefaultCameraDistanceLimit = 50.00
pid = 0

Attached = False
versionChecked = False

tmpPtr = 0x0
nameplate_offset = (0x0)
namePlateAddSwitchFrom = (ctypes.c_byte * 8)(216, 29, 136, 217, 196, 0, 221, 216)
namePlateAddSwitchTo = (ctypes.c_byte * 8)(216, 29, 216, 254, 128, 0, 221, 216)
fov_offset = (0x0)
camDist_offset = [0] * 2 # 1: Camera Distance Limit, 2: Camera Distance
maxcamDist_offset = [0] * 2 # 1: Max Camera Distance, 2: MaxCameraDistance Variable (/console CameraDistanceMax N)

config_file = configparser.ConfigParser()

isVanilla = False
isTBC = False
isWotLK = False

warningText = "========== Need to Enter in the World First Then Use This Tool ========="

'''
======================================================= Code For Processes =======================================================
'''
def LOWORD(dword):
    return dword & 0x0000ffff
def HIWORD(dword): 
    return dword >> 16
def get_file_version(path):
    pe = pefile.PE(path)
    # print (pe.dump_info())
    ms = pe.VS_FIXEDFILEINFO[0].FileVersionMS
    ls = pe.VS_FIXEDFILEINFO[0].FileVersionLS
    return HIWORD (ms), LOWORD (ms), HIWORD (ls), LOWORD (ls)

# def get_version_number (filename):
#   info = GetFileVersionInfo (filename, "\\")
#   print (info)
#   ms = info['FileVersionMS']
#   ls = info['FileVersionLS']
#   return HIWORD (ms), LOWORD (ms), HIWORD (ls), LOWORD (ls)

def getpids(proc_name):
    proc_ignore_list = ["wowreeb"]
    pid_list = []
    for proc in psutil.process_iter():
        if proc_name.lower() in proc.name().lower():
            for ignore in proc_ignore_list:
                if not proc.name().lower().__contains__(ignore.lower()):
                    # make pid a list of pids
                    pid_list.append(proc.name() + " [" + str(proc.pid) + "]")                
    return pid_list

'''
======================================================= Code For Extra Fns =======================================================
'''
def read_file():
    global config_file
    if not os.path.exists('VanillaSSinn3rConfig.ini'):
        print ("Config file not found. Creating new config file.")
        config_file['Settings'] = {
            'nameplate_check': 'False',
            'nameplate_range': '20.00',
            'fov_check': 'False', 
            'fov_value': '1.5707963267948966',
            'camDist_check': 'False',
            'camDist_value': '50.00',
            }
        write_file()
    config_file.read('VanillaSSinn3rConfig.ini')
    if not config_file.has_section('Settings'):
        print ("Config file is missing settings. Creating new config file.")
        config_file['Settings'] = {
            'nameplate_check': 'False',
            'nameplate_range': '20.00',
            'fov_check': 'False', 
            'fov_value': '1.5707963267948966',
            'camDist_check': 'False',
            'camDist_value': '50.00',
            }
        write_file()
    return
def write_file():
    global config_file
    config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
def str2bool(v):
  return v.lower() in ("yes", "true", "t", "1")

'''
======================================================= Code For Main HACKS =======================================================
'''
def Hacks():
    global Attached
    global GameVersion
    global nameplate_offset
    global fov_offset
    global config_file
    global GameDefaultNameplateRange
    global GameDefaultFOV
    global versionChecked
    global isVanilla
    global isTBC
    global isWotLK
    global tmpPtr
    global camDist_offset
    global maxcamDist_offset
    global GameDefaultCameraDistance
    global GameDefaultCameraDistanceLimit
    try:
        with Process.open_process(pid) as p:
            if GameVersion.__contains__("5875"):
                if not isVanilla:
                    # Switching Nameplate Render Address To FarClip Evaluator From Nameplate Evaluator
                    p.write_memory(0x60F7B8, namePlateAddSwitchTo)
                    isVanilla = True
                # 0xC4D988 hex(12900744) Detected in vMaNGOS
                nameplate_offset = (0x80FED8) # 0x80FED8 hex(8453848) Now using farclip address for manipulating nameplate range
                fov_offset = (0x8089B4) # 0x8089B4 hex(8423860)
                maxcamDist_offset[0] = (0x8089A4) # 0x8089A4 hex(8423844)

                tmpPtr = getAddress(0xBE0E6C, { 0x24 })
                if (tmpPtr != 0x0):
                    maxcamDist_offset[1] = tmpPtr
                tmpPtr = getAddress(0xB4B2BC, { 0x65B8, 0x198 })
                if (tmpPtr != 0x0):
                    camDist_offset[0] = tmpPtr
                tmpPtr = getAddress(0xB4B2BC, { 0x65B8, 0xEC })
                if (tmpPtr != 0x0):
                    camDist_offset[1] = tmpPtr
                if not versionChecked:
                    Print("[>] World of Warcraft [" + GameVersion + "] detected!")
                    GameDefaultNameplateRange = (p.read_memory(nameplate_offset, ctypes.c_float())).value
                    GameDefaultFOV = (p.read_memory(fov_offset, ctypes.c_float())).value
                    if (tmpPtr != 0x0):
                        GameDefaultCameraDistanceLimit = (p.read_memory(maxcamDist_offset[0], ctypes.c_float())).value # Before : Read(camDist_offset[0])
                        GameDefaultCameraDistance = (p.read_memory(camDist_offset[1], ctypes.c_float())).value
                    else:
                        GameDefaultCameraDistanceLimit = 50.00
                        GameDefaultCameraDistance = 50.00
                    versionChecked = True
            elif GameVersion.__contains__("8606"):
                if not isTBC:
                    isTBC = True

                nameplate_offset = (0xBA4B90) # 0xBA4B90 hex(12209040)
                fov_offset = (0x8b5a04) # 0x8b5a04 hex(9132548)
                maxcamDist_offset[0] = (0x8B59F4) # 0x8B59F4 hex(9132532)

                tmpPtr = getAddress(0xCF3204, { 0x24 })
                if (tmpPtr != 0x0):
                    maxcamDist_offset[1] = tmpPtr
                tmpPtr = getAddress(0xC6ECCC, { 0x732C, 0x1B4 })
                if (tmpPtr != 0x0):
                    camDist_offset[0] = tmpPtr
                tmpPtr = getAddress(0xC6ECCC, { 0x732C, 0x100 })
                if (tmpPtr != 0x0):
                    camDist_offset[1] = tmpPtr
                if not versionChecked:
                    Print("[>] World of Warcraft [" + GameVersion + "] detected!")
                    GameDefaultNameplateRange = (p.read_memory(nameplate_offset, ctypes.c_float())).value
                    GameDefaultFOV = (p.read_memory(fov_offset, ctypes.c_float())).value
                    if (tmpPtr != 0x0):
                        GameDefaultCameraDistanceLimit = (p.read_memory(maxcamDist_offset[0], ctypes.c_float())).value # Before : Read(camDist_offset[0])
                        GameDefaultCameraDistance = (p.read_memory(camDist_offset[1], ctypes.c_float())).value
                    else:
                        GameDefaultCameraDistanceLimit = 50.00
                        GameDefaultCameraDistance = 50.00
                    versionChecked = True
            elif GameVersion.__contains__("12340"):
                if not isWotLK:
                    isWotLK = True

                nameplate_offset = (0xADAA7C) # 0xADAA7C hex(10390920)
                fov_offset = (0x9e8d88) # 0x9e8d88 hex(9132548)
                maxcamDist_offset[0] = (0xA1E2FC) # 0xA1E2FC hex(10609404)

                tmpPtr = getAddress(0xC2498C, { 0x2C })
                if (tmpPtr != 0x0):
                    maxcamDist_offset[1] = tmpPtr
                tmpPtr = getAddress(0xB7436C, { 0x7E20, 0x1E8 })
                if (tmpPtr != 0x0):
                    camDist_offset[0] = tmpPtr
                tmpPtr = getAddress(0xB7436C, { 0x7E20, 0x118 })
                if (tmpPtr != 0x0):
                    camDist_offset[1] = tmpPtr
                if not versionChecked:
                    Print("[>] World of Warcraft [" + GameVersion + "] detected!")
                    GameDefaultNameplateRange = (p.read_memory(nameplate_offset, ctypes.c_float())).value
                    GameDefaultFOV = (p.read_memory(fov_offset, ctypes.c_float())).value
                    if (tmpPtr != 0x0):
                        GameDefaultCameraDistanceLimit = (p.read_memory(maxcamDist_offset[0], ctypes.c_float())).value # Before : Read(camDist_offset[0])
                        GameDefaultCameraDistance = (p.read_memory(camDist_offset[1], ctypes.c_float())).value
                    else:
                        GameDefaultCameraDistanceLimit = 50.00
                        GameDefaultCameraDistance = 50.00
                    versionChecked = True
            else:
                Print("[>] World of Warcraft [" + GameVersion + "] is not supported.")
                Attached = False
                Print("[>] Detached from WoW [" + wow_process.get() + "]")
            p.close()
    except Exception as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside Hacks Catch")

def LoadConfig():
    global config_file
    global namePlateRangeSlider
    try:
        read_file()
        namePlateCheckBoxVar.set(str2bool(config_file['Settings']['nameplate_check']))
        fovCheckBoxVar.set(str2bool(config_file['Settings']['fov_check']))
        camDistCheckBoxVar.set(str2bool(config_file['Settings']['camDist_check']))
        if namePlateCheckBoxVar.get():
            namePlateRangeSlider.set(float(config_file['Settings']['nameplate_range']))
        if fovCheckBoxVar.get():
            fov_textBox.delete(0, END)
            fov_textBox.insert(0, float(config_file['Settings']['fov_value']))
        if camDistCheckBoxVar.get():
            camDistSlider.set(float(config_file['Settings']['camDist_value']))
    except Exception as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside LoadConfig Catch")
    return

def Vanilla_Extra_Hacks():
    # Checkbox for Background Sound
    global bgSoundCheckBoxVar
    global bgSoundCheckBox
    bgSoundCheckBoxVar = BooleanVar(False)
    bgSoundCheckBox = ttk.Checkbutton(master, text="Sound in Background", variable=bgSoundCheckBoxVar, command=bgSoundCheckBox_Clicked)
    bgSoundCheckBox.grid(row=6, column=0, columnspan=100, sticky=W)
    # Checkbox for Quickloot
    global quicklootCheckBoxVar
    global quicklootCheckBox
    quicklootCheckBoxVar = BooleanVar(False)
    quicklootCheckBox = ttk.Checkbutton(master, text="Quickloot", variable=quicklootCheckBoxVar, command=quicklootCheckBox_Clicked)
    quicklootCheckBox.grid(row=6, column=1, columnspan=100, sticky=W)

def TBC_Extra_Hacks():
    pass
def WotLK_Extra_Hacks():
    pass
def LoadExtraHacks():
    global Attached
    global isVanilla
    global isTBC
    global isWotLK
    if Attached:
        if isVanilla:
            master.geometry("600x420")
            Vanilla_Extra_Hacks()

'''
======================================================== Functions for Extra UI ========================================================
'''
def bgSoundCheckBox_Clicked():
    global Attached
    global bgSoundCheckBoxVar
    if Attached:
        with Process.open_process(pid) as p:
            if bgSoundCheckBoxVar.get():
                p.write_memory(0x7A4869, ctypes.c_byte(39))
                # bgSoundCheckBox.config(bg="light green")
                Print("[>] Enabled: Sound in Background")
            else:
                p.write_memory(0x7A4869, ctypes.c_byte(20))
                # bgSoundCheckBox.config(bg=defaultbg)
                Print("[>] Disabled: Sound in Background")
            p.close()

def quicklootCheckBox_Clicked():
    global Attached
    global quicklootCheckBoxVar
    if Attached:
        with Process.open_process(pid) as p:
            if quicklootCheckBoxVar.get():
                p.write_memory(0x4C1ECF, ctypes.c_byte(117))
                p.write_memory(0x4C2B25, ctypes.c_byte(117))
                # quicklootCheckBox.config(bg="light green")
                Print("[>] Enabled: Quickloot")
            else:
                p.write_memory(0x4C1ECF, ctypes.c_byte(116))
                p.write_memory(0x4C2B25, ctypes.c_byte(116))
                # quicklootCheckBox.config(bg=defaultbg)
                Print("[>] Disabled: Quickloot")
            p.close()

'''
======================================================== Functions for UI ========================================================
'''
def Print(msg):
    infobox.config(state=NORMAL)
    infobox.insert(tk.END, msg + "\n")
    infobox.see(tk.END)
    infobox.config(state=DISABLED)
    return

def infoPrint(forText, value):
    if forText.__contains__("nameplate"):
        Print("[>] Nameplate Range: " + str("{:.2f}".format(value)) + " yards")
    elif (forText.__contains__("fov")):
        Print("[>] FOV: " + str("{:.6f}".format(value)) + "	(Use In Game: \' /console ReloadUI \')")
    elif (forText.__contains__("camera")):
        Print("[>] Camera Distance: " + str("{:.2f}".format(value)))
    return

def ClearLogWin():
    infobox.config(state=NORMAL)
    infobox.delete(1.0, END)
    infobox.config(state=DISABLED)
    return

def Init():
    try:
        pids = getpids(process_text.get())
        wow_process['values'] = (pids)
        wow_process.current(0)
    except:
        pass

def scan_btn_clicked():
    Init()

def attach_btn_clicked():
    global Attached
    global GameVersion
    global nameplate_offset
    global fov_offset
    global config_file
    global GameDefaultNameplateRange
    global GameDefaultFOV
    global versionChecked
    global namePlateRangeSlider
    global pid
    global isVanilla
    global isTBC
    global isWotLK
    global namePlateCheckBoxVar
    global fovCheckBoxVar
    global tmpPtr
    if Attached:
        Print("[~>] We are already attached to WoW")
        return
    if wow_process.current() == -1:
        Print("[~>] Please select a WoW process")
        return
    try:
        pid = int(wow_process.get().split("[")[1].split("]")[0])
        getAddress(0xBE0E6C, {0x24})
        with Process.open_process(pid) as p:
            ClearLogWin()
            Print("[>] Attaching to " + wow_process.get())
            file_path = os.path.realpath(re.sub("\\Device\\\\HarddiskVolume[0-9]\\\\", "", p.get_path()))
            try:
                GameVersion = ".".join ([str (i) for i in get_file_version(file_path)])
                LoadConfig()
            except Exception as e:
                if debugCheckBoxVar.get():
                    Print(e.__str__())
                    Print("[>>>>] Debug: Inside attach_btn_click Catch")
            Attached = True
            Print("[>] Successfully attached to WoW [" + wow_process.get() + "]")
            Hacks()
            if Attached:
                if namePlateCheckBoxVar.get():
                    p.write_memory(nameplate_offset, ctypes.c_float(math.pow(float(config_file['Settings']['nameplate_range']), 2)))
                    namePlateRangeSlider.set(math.sqrt((p.read_memory(nameplate_offset, ctypes.c_float())).value))
                    infoPrint("nameplate", int(namePlateRangeSlider.get()))
                if fovCheckBoxVar.get():
                    p.write_memory(fov_offset, ctypes.c_float(float(config_file['Settings']['fov_value'])))
                    fov_textBox.delete(0, END)
                    fov_textBox.insert(0, (p.read_memory(fov_offset, ctypes.c_float())).value)
                    infoPrint("fov", float(fov_textBox.get()))
                if camDistCheckBoxVar.get():
                    try:
                        p.write_memory(maxcamDist_offset[0], ctypes.c_float(float(config_file['Settings']['camDist_value'])))
                        if (tmpPtr != 0x0):
                            p.write_memory(maxcamDist_offset[1], ctypes.c_float(float(config_file['Settings']['camDist_value'])))
                            p.write_memory(camDist_offset[0], ctypes.c_float(float(config_file['Settings']['camDist_value'])))
                            p.write_memory(camDist_offset[1], ctypes.c_float(float(config_file['Settings']['camDist_value'])))
                            camDistSlider.set((p.read_memory(camDist_offset[0], ctypes.c_float())).value)
                            infoPrint("camera", int(camDistSlider.get()))
                    except Exception as e:
                        if debugCheckBoxVar.get():
                            Print(warningText)
                            print(e.__str__())
            # We don't actually have to read the value here, but let's do so anyways...
            # print((p.read_memory(nameplate_offset, ctypes.c_float())).value)
            # print((p.read_memory(fov_offset, ctypes.c_float())).value)
            p.close()
        LoadExtraHacks()
    except NameError as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside attach_btn_clicked Catch")
    return
def save_btn_clicked():
    saveSettings()
    Print("========================== Settings Saved ========================")
    return
def reset_btn_clicked():
    global Attached
    global GameDefaultNameplateRange
    global GameDefaultFOV
    if Attached:
        ClearLogWin()
        Print("=============== Resetting Values To Game Default/Original ============")
        save_n_reset()
        infoPrint("nameplate", math.sqrt(GameDefaultNameplateRange))
        infoPrint("fov", GameDefaultFOV)
        if (tmpPtr != 0x0):
            infoPrint("camera", GameDefaultCameraDistanceLimit)
        Print("============================ Detached ===========================")
    return
def namePlateRangeSlider_Scroll(event):
    global Attached
    global config_file
    global namePlateRangeSlider
    global namePlateCheckBoxVar
    if Attached and namePlateCheckBoxVar.get():
        with Process.open_process(pid) as p:
            p.write_memory(nameplate_offset, ctypes.c_float(math.pow(float(int(namePlateRangeSlider.get())), 2)))
            config_file['Settings']['nameplate_range'] = str(int(namePlateRangeSlider.get()))
            infoPrint("nameplate", int(namePlateRangeSlider.get()))
            config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
            p.close()
def namePlateCheckBox_Changed():
    global Attached
    global config_file
    if Attached:
        if namePlateCheckBoxVar.get():
            config_file['Settings']['nameplate_check'] = "True"
        else:
            config_file['Settings']['nameplate_check'] = "False"
        config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
        namePlateRangeSlider.set(float(config_file['Settings']['nameplate_range']))
def fov_btn_clicked():
    global Attached
    global config_file
    global fovCheckBoxVar
    if Attached and fovCheckBoxVar.get():
        with Process.open_process(pid) as p:
            p.write_memory(fov_offset, ctypes.c_float(float(fov_textBox.get())))
            config_file['Settings']['fov_value'] = str(float(fov_textBox.get()))
            infoPrint("fov", float(fov_textBox.get()))
            config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
            p.close()
def fovCheckBox_Changed():
    global Attached
    global config_file
    if Attached:
        if fovCheckBoxVar.get():
            config_file['Settings']['fov_check'] = "True"
        else:
            config_file['Settings']['fov_check'] = "False"
        config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
        fov_textBox.delete(0, END)
        fov_textBox.insert(0, float(config_file['Settings']['fov_value']))

def camDistSlider_Scroll(event):
    global Attached
    global config_file
    global camDistSlider
    global camDist_offset
    global maxcamDist_offset
    global tmpPtr
    try:
        if Attached and camDistCheckBoxVar.get():
            with Process.open_process(pid) as p:
                p.write_memory(maxcamDist_offset[0], ctypes.c_float(float(int(camDistSlider.get()))))
                Hacks()
                if (tmpPtr != 0x0):
                    p.write_memory(maxcamDist_offset[1], ctypes.c_float(float(int(camDistSlider.get()))))
                    p.write_memory(camDist_offset[0], ctypes.c_float(float(int(camDistSlider.get()))))
                    p.write_memory(camDist_offset[1], ctypes.c_float(float(int(camDistSlider.get()))))

                config_file['Settings']['camdist_value'] = str(float(int(camDistSlider.get())))
                config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
                infoPrint("camera", float(int(camDistSlider.get())))
                p.close()
    except Exception as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside CamDistSlider Catch")

def cameraCheckBox_Changed():
    global Attached
    global config_file
    if Attached:
        if camDistCheckBoxVar.get():
            config_file['Settings']['camdist_check'] = "True"
        else:
            config_file['Settings']['camdist_check'] = "False"
        config_file.write(open('VanillaSSinn3rConfig.ini', 'w'))
        camDistSlider.set(float(config_file['Settings']['camdist_value']))
    
'''
============================================================ Code For Indirectly Related Fns ===========================================================
'''
def save_n_reset():
    global Attached
    global versionChecked
    global isVanilla
    global nameplate_offset
    global fov_offset
    global config_file
    global GameDefaultNameplateRange
    global GameDefaultFOV
    global namePlateAddSwitchFrom
    global tmpPtr
    Hacks()
    Attached = False
    versionChecked = False
    saveSettings()
    try:
        with Process.open_process(pid) as p:
            if isVanilla:
                # Switching Back Nameplate Render Address To Nameplate Evaluator From FarClip Evaluator
                p.write_memory(0x60F7B8, namePlateAddSwitchFrom)
                # ============================================================================
                Attached = True
                bgSoundCheckBoxVar.set(False)
                # called this function to reset the bg sound memory value
                bgSoundCheckBox_Clicked()
                bgSoundCheckBox.destroy()

                quicklootCheckBoxVar.set(False)
                # called this function to reset the quickloot memory value
                quicklootCheckBox_Clicked()
                quicklootCheckBox.destroy()
                Attached = False
                # ============================================================================
                master.geometry(ui_default_size)
                isVanilla = False
            p.write_memory(nameplate_offset, ctypes.c_float(GameDefaultNameplateRange))
            p.write_memory(fov_offset, ctypes.c_float(GameDefaultFOV))
            p.write_memory(maxcamDist_offset[0], ctypes.c_float(GameDefaultCameraDistanceLimit))
            if (tmpPtr != 0x0):
                p.write_memory(maxcamDist_offset[1], ctypes.c_float(GameDefaultCameraDistanceLimit))
                p.write_memory(camDist_offset[0], ctypes.c_float(GameDefaultCameraDistanceLimit))
                p.write_memory(camDist_offset[1], ctypes.c_float(GameDefaultCameraDistance))
            p.close()
    except NameError as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside save_n_reset Catch")
    return
def saveSettings():
    global config_file
    read_file()
    if (namePlateCheckBoxVar.get()):
        config_file['Settings']['nameplate_range'] = str(int(namePlateRangeSlider.get()))
    if (fovCheckBoxVar.get()):
        config_file['Settings']['fov_value'] = str(fov_textBox.get())
    if (camDistCheckBoxVar.get()):
        config_file['Settings']['camdist_value'] = str(int(camDistSlider.get()))
    config_file['Settings']['nameplate_check'] = str(namePlateCheckBoxVar.get())
    config_file['Settings']['fov_check'] = str(fovCheckBoxVar.get())
    config_file['Settings']['camdist_check'] = str(camDistCheckBoxVar.get())
    write_file()
    return

def getAddress(baseM, offsets):
    try:
        Base = baseM
        with Process.open_process(pid) as p:
            for ptr in offsets:
                tmp = p.read_memory(Base, ctypes.c_int()).value
                if (tmp != 0):
                    Base = tmp + ptr
                else:
                    raise Exception("Address Not Found")
            p.close()
            return Base
    except Exception as e:
        if debugCheckBoxVar.get():
            Print(e.__str__())
            Print("[>>>>>] Debug: Inside getAddress Catch")
        return 0x0

def on_closing():
    global Attached
    try:
        if Attached:
            save_n_reset()
        master.destroy()
    except:
        master.destroy()
        exit()
    return

'''
============================================================ Code For GUI ===========================================================
'''
if __name__ == "__main__":
    master = tk.Tk()
    sv_ttk.set_theme("dark") # dark theme
    # master.iconphoto(True, tk.PhotoImage(file='logo.png'))
    # master.iconbitmap('logo.ico')
    # defaultbg = master.cget('bg')
    master.title('VanillaSSinn3r - WoW Vanilla/TBC/WotLK Tool By SSinist3r')
    master.geometry(ui_default_size)
    master.resizable(False, False)

    # Textbox for Logs
    infobox = Text(master, height = 10, width = 60, bg = "black", fg = "light green", font = ("Prototype", 12, "bold"), state=DISABLED)
    infobox.grid(row=0, columnspan=200, column=0, padx=8, pady=10, sticky=N+S+E+W)
    # create a Scrollbar and associate it with txt
    scrollb = ttk.Scrollbar(command=infobox.yview)
    scrollb.grid(row=0, columnspan=200, column=5, sticky='ns', padx=0, pady=10)
    infobox['yscrollcommand'] = scrollb.set

    # Label, Textbox, Button for Process Name Scan
    L1 = ttk.Label(master, text="Process Name : ").grid(row=1, column=0)
    process_text = StringVar()
    process_textBox = ttk.Entry(master, width = 30, textvariable = process_text)
    process_textBox.insert(0, "WoW")
    process_textBox.grid(row=1, column=1)
    scan_btn = ttk.Button(master, text='Scan', command=scan_btn_clicked, width=5).grid(row=1, column=2)

    # Label, ComboBox, Button for WoW Process Attach
    L2 = ttk.Label(master, text="WoW Process : ").grid(row=2, column=0)
    n = StringVar()
    wow_process = ttk.Combobox(master, width = 27, textvariable = n)
    wow_process.grid(row=2, column=1, padx=5, pady=5)
    wow_process.current()
    attach_btn = ttk.Button(master, text='Attach', command=attach_btn_clicked, width=8).grid(row=2, column=2, pady=5)

    # Button for Save & Reset
    reset_btn = ttk.Button(master, text='Reset', command=reset_btn_clicked, width=5).grid(row=2, column=3, padx=5)
    save_btn = ttk.Button(master, text='Save', command=save_btn_clicked, width=5).grid(row=2, column=4, padx=0)

    # Label, Slider, Button for Nameplate Range
    namePlateCheckBoxVar = BooleanVar(False)
    namePlateCheckBox = ttk.Checkbutton(master, text='Nameplate Range: ', variable=namePlateCheckBoxVar, command=namePlateCheckBox_Changed)
    namePlateCheckBox.grid(row=3, column=0, sticky=W)
    namePlateRangeSlider = ttk.Scale(master, command=namePlateRangeSlider_Scroll, from_=0, to=1000, orient=HORIZONTAL, length=185)
    namePlateRangeSlider.grid(row=3, column=1)

    # Label, Textbox, Button for FOV
    fovCheckBoxVar = BooleanVar(False)
    fovCheckBox = ttk.Checkbutton(master, text='FoV Set Value: ', variable=fovCheckBoxVar, command=fovCheckBox_Changed)
    fovCheckBox.grid(row=4, column=0, sticky=W)
    fov_text = StringVar()
    fov_textBox = ttk.Entry(master, width = 30, textvariable = fov_text)
    fov_textBox.insert(0, "1.5707963267948966")
    fov_textBox.grid(row=4, column=1)
    fov_btn = ttk.Button(master, text='Set', command=fov_btn_clicked, width=5).grid(row=4, column=2)

    # Checkbox, Slider for Camera Distance
    camDistCheckBoxVar = BooleanVar(False)
    camDistCheckBox = ttk.Checkbutton(master, text='Camera Distance: ', variable=camDistCheckBoxVar, command=cameraCheckBox_Changed)
    camDistCheckBox.grid(row=5, column=0, sticky=W)
    camDistSlider = ttk.Scale(master, command=camDistSlider_Scroll, from_=0, to=1000, orient=HORIZONTAL, length=185)
    camDistSlider.grid(row=5, column=1)

    # Checkbox for Debug Mode
    debugCheckBoxVar = BooleanVar(False)
    debugCheckBox = ttk.Checkbutton(master, text='Debug Mode', variable=debugCheckBoxVar)
    debugCheckBox.grid(row=1, column=3, columnspan=1000, sticky=W)

    Print("=============== Run with Admin/Super User Privileges ===============")
    Print(warningText)
    Init()
    master.protocol("WM_DELETE_WINDOW", on_closing)
    master.mainloop()