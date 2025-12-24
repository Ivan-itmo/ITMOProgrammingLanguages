import ctypes
import ctypes.util
import os

if os.name == 'nt':
    user32 = ctypes.windll.user32
    user32.MessageBoxW.argtypes = [ctypes.c_void_p, ctypes.c_wchar_p, ctypes.c_wchar_p, ctypes.c_uint]
    user32.MessageBoxW.restype = ctypes.c_int
    user32.MessageBoxW(0, "Привет из Python!", "ctypes Демо", 0x40)
else:
    libc = ctypes.CDLL(ctypes.util.find_library('c'))
    libc.puts.argtypes = [ctypes.c_char_p]
    libc.puts.restype = ctypes.c_int
    libc.puts(b"Hello from C libc!")