import sys
import os
import ctypes.util

def find_library(name):
    return ctypes.util.find_library(name)

def print_library_path(library_name):
    lib_path = find_library(library_name)
    if lib_path:
        print(f"Found {library_name}: {lib_path}")
    else:
        print(f"{library_name} not found")

print("Python executable path:", sys.executable)
print("Python library paths:")
for path in sys.path:
    print(path)

print("\nChecking for libpython3.9.dylib using ctypes.util.find_library:")
print_library_path('python3.9')

print("\nManually searching for libpython3.9.dylib in common library paths:")
common_paths = [
    '/usr/local/lib', '/usr/lib', '/lib', '/opt/local/lib',
    '/Volumes/Rooster_SSD/Anaconda/anaconda3/envs/cybergod/lib'
]
found = False
for lib_path in common_paths:
    if os.path.isdir(lib_path):
        for file in os.listdir(lib_path):
            if file.startswith("libpython3.9") and file.endswith(".dylib"):
                found = True
                print(f"Found dynamic library: {os.path.join(lib_path, file)}")
if not found:
    print("libpython3.9.dylib not found in common library paths.")