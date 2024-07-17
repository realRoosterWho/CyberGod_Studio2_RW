import os
import sys
import cv2
import shapely
import mediapipe
import cvzone

def get_library_paths():
    lib_paths = set()

    # Function to add paths from a module
    def add_module_libs(module):
        module_path = os.path.dirname(module.__file__)
        lib_paths.add(module_path)
        if hasattr(module, 'LIBRARY_PATH'):
            lib_paths.add(module.LIBRARY_PATH)

    # Adding paths from different modules
    add_module_libs(cv2)
    add_module_libs(shapely)
    add_module_libs(mediapipe)
    add_module_libs(cvzone)

    # Print the library paths
    for path in lib_paths:
        print(f"Library path: {path}")

    # Check for additional shared libraries in .libs directories
    libs_dirs = [os.path.join(path, ".libs") for path in lib_paths]
    for libs_dir in libs_dirs:
        if os.path.exists(libs_dir):
            for root, dirs, files in os.walk(libs_dir):
                for file in files:
                    print(f"Shared library: {os.path.join(root, file)}")

if __name__ == "__main__":
    get_library_paths()
