import tkinter as tk
from tkinter import ttk
from tkinter import filedialog, messagebox  # Import filedialog and messagebox for file selection and messages

import cv2
from PIL import Image, ImageTk
import threading
import queue
import sys
import os
from motion import MotionCapture  # Import the MotionCapture class
import threading
import time
import subprocess

class Logger:
    def __init__(self, log_function):
        self.log_function = log_function

    def write(self, message):
        if message.strip():  # 忽略空消息
            self.log_function(message)

    def flush(self):
        pass  # 需要实现，但可以留空

class CameraApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Camera Selector")
        self.motion_capture = None  # Initialize motion_capture to None
        self.correct_position_flag = False  # Flag to track position status

        # Start the output monitoring thread with log pattern detection
        self.monitoring_thread = threading.Thread(target=self.monitor_output, daemon=True)
        self.monitoring_thread.start()

        # Initialize log queue before calling get_camera_list
        self.log_queue = queue.Queue()
        self.log("Application started.")

        # Default game path based on platform
        base_path = os.path.dirname(os.path.realpath(sys.argv[0]))
        self.game_path = os.path.join(base_path, "CyberSpirit.app" if sys.platform == "darwin" else "CyberSpirit.exe")

        # Set initial window size
        initial_width = 480  # Increase the width
        initial_height = 700  # Increase the height
        self.root.geometry(f"{initial_width}x{initial_height}")
        self.root.minsize(initial_width, initial_height)

        # Configure grid layout
        self.root.columnconfigure(0, weight=1)
        self.root.columnconfigure(1, weight=1) #这是用来设置第一列和第二列的宽度比例

        self.root.rowconfigure(9, weight=1)

        # Instruction label
        self.instruction_label = tk.Label(root, text="Please Select Camera", font=("Helvetica", 16), fg="red")
        self.instruction_label.grid(row=0, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # Start Game Button and Status Label
        self.start_game_button = tk.Button(root, text="Start Game", command=self.start_game, state="disabled", font=("Helvetica", 14))
        self.start_game_button.grid(row=1, column=0, padx=5, pady=5, sticky="e")

        #设置一个嵌套的frame，用来同时搞定game_status_label和toggle_button
        self.game_path_frame = tk.Frame(root)
        self.game_path_frame.grid(row=1, column=1, padx=5, pady=5, sticky="w")


        self.game_status_label = tk.Label(self.game_path_frame, text="Please follow instruction to setup.", fg="red", font=("Helvetica", 10))
        self.game_status_label.grid(row=0, column=1, padx=5, pady=5, sticky="w")

        # Toggle button to show/hide the collapsible frame
        self.toggle_button = tk.Button(self.game_path_frame, text="▼ Expand", command=self.toggle_collapse, font=("Helvetica", 10),width=10)
        self.toggle_button.grid(row=0, column=0, padx=5, pady=5, sticky="w")

        # Collapsible frame for manual path selection
        self.collapse_frame = tk.Frame(root)
        self.collapse_frame.grid(row=2, column=0, columnspan=2, sticky="nsew")
        self.collapse_frame.grid_remove()  # Start collapsed

        # Path selection label and entry field
        self.path_instruction_label = tk.Label(self.collapse_frame,
                                               text="Please select the file path. The file should be named CyberSpirit.app or CyberSpirit.exe",
                                               font=("Helvetica", 10))
        self.path_instruction_label.grid(row=0, column=0, columnspan=2, sticky="e", padx=5, pady=5)

        self.select_path_button = tk.Button(self.collapse_frame, text="Select Path", command=self.select_game_path,
                                            width=10)
        self.select_path_button.grid(row=1, column=0, padx=5, pady=5, sticky="e")

        # Create a label to display the path
        self.path_label = tk.Label(self.collapse_frame, text=self.game_path, font=("Helvetica", 10), anchor="w", wraplength=300)
        self.path_label.grid(row=1, column=1, padx=5, pady=5, sticky="ew")




        # Load the image
        base_path = getattr(sys, '_MEIPASS', os.path.abspath("."))
        logo_path = os.path.join(base_path, "Logo_cyberspirit.png")
        self.logo_image = Image.open(logo_path)
        self.logo_image = self.logo_image.resize((100, 100), Image.LANCZOS)
        self.logo_photo = ImageTk.PhotoImage(self.logo_image)

        # Create a label for the image
        self.logo_label = tk.Label(root, image=self.logo_photo)
        self.logo_label.grid(row=4, column=0, padx=5, pady=5, sticky="nsew")

        self.camera_label = tk.Label(root, text="Select Camera:")
        self.camera_label.grid(row=4, column=1, padx=5, pady=5, sticky="e")

        self.camera_listbox = tk.Listbox(root, selectmode=tk.SINGLE, height=5)
        self.camera_listbox.grid(row=4, column=1, padx=5, pady=5, sticky="we")


        for camera in self.get_camera_list():
            self.camera_listbox.insert(tk.END, camera)

        self.start_button = tk.Button(root, text="Start Camera", command=self.start_camera)
        self.start_button.grid(row=5, column=0, padx=5, pady=5, sticky="e")

        self.stop_button = tk.Button(root, text="Stop Camera", command=self.stop_camera, state="disabled")
        self.stop_button.grid(row=5, column=1, padx=5, pady=5, sticky="w")

        self.start_motion_button = tk.Button(root, text="Start Motion Capture", command=self.start_motion_capture, state="disabled")
        self.start_motion_button.grid(row=6, column=0, padx=5, pady=5, sticky="e")

        self.stop_motion_button = tk.Button(root, text="Stop Motion Capture", command=self.stop_motion_capture, state="disabled")
        self.stop_motion_button.grid(row=6, column=1, padx=5, pady=5, sticky="w")

        # Log title
        self.log_title = tk.Label(root, text="Log", font=("Helvetica", 12))
        self.log_title.grid(row=7, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # Log text box
        self.log_text = tk.Text(root, height=5, state="normal")  # Make sure it's editable initially
        self.log_text.grid(row=8, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
        self.log_text.config(state="disabled")  # Set to disabled after initial setup

        # Video display label with initial size
        self.video_label = tk.Label(root, width=initial_width, height=initial_height)
        self.video_label.grid(row=9, column=0, columnspan=2, sticky="nsew")

        # Start the log update loop
        self.update_log()

        self.cap = None
        self.root.protocol("WM_DELETE_WINDOW", self.on_closing)

        # Redirect stdout to log_text
        sys.stdout = Logger(self.log)

    def get_camera_list(self):
        index = 0
        arr = []
        while True:
            cap = cv2.VideoCapture(index)
            if not cap.read()[0]:
                break
            else:
                width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
                height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
                camera_name = f"Camera {index} - {width}x{height}"
                arr.append(camera_name)
                self.log(camera_name)  # Log the camera name
            cap.release()
            index += 1
        return arr

    def start_camera(self):
        if self.cap:
            self.stop_camera()  # Stop the current camera if already running
        selected_camera_index = self.camera_listbox.curselection()
        if selected_camera_index:
            selected_camera = self.camera_listbox.get(selected_camera_index)
            camera_index = int(selected_camera.split()[1])
            self.cap = cv2.VideoCapture(camera_index)
            if self.cap.isOpened():
                self.update_frame()
                self.stop_button.config(state="normal")
                self.start_button.config(state="disabled")
                self.start_motion_button.config(state="normal")
                self.log("Camera started.")
                self.instruction_label.config(text="Please open motion capture", fg="red")
            else:
                self.cap.release()
                self.cap = None
                self.log("Failed to open the selected camera.")

    def stop_camera(self):
        if self.cap:
            self.cap.release()
            self.cap = None
            self.video_label.config(image="")
            self.stop_button.config(state="disabled")
            self.start_button.config(state="normal")
            self.start_motion_button.config(state="disabled")
            self.stop_motion_capture()
            self.log("Camera stopped.")
            self.instruction_label.config(text="Please Select Camera", fg="red")

    # 修改 start_motion_capture 方法
    def start_motion_capture(self):
        if self.cap and not self.motion_capture:
            self.stop_camera()  # Stop the camera input
            selected_camera_index = self.camera_listbox.curselection()
            if selected_camera_index:
                selected_camera = self.camera_listbox.get(selected_camera_index)
                camera_index = int(selected_camera.split()[1])
                self.motion_capture = MotionCapture(camera_index, frame_callback=self.update_motion_frame)

                threading.Thread(target=self.motion_capture.start, daemon=True).start()
                self.start_motion_button.config(state="disabled")
                self.stop_motion_button.config(state="normal")
                self.log("Motion capture started.")
                self.instruction_label.config(text="Motion Capture On", fg="red")

    def update_motion_frame(self, img):
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = Image.fromarray(img)

        # Resize the frame to fit label size while maintaining aspect ratio
        label_width = self.video_label.winfo_width()
        label_height = self.video_label.winfo_height()
        frame_width, frame_height = img.size

        label_ratio = label_width / label_height
        frame_ratio = frame_width / frame_height

        if label_ratio > frame_ratio:
            new_height = label_height
            new_width = int(new_height * frame_ratio)
        else:
            new_width = label_width
            new_height = int(new_width / frame_ratio)

        img = img.resize((new_width, new_height), Image.LANCZOS)
        imgtk = ImageTk.PhotoImage(image=img)
        self.video_label.imgtk = imgtk
        self.video_label.configure(image=imgtk)

    def stop_motion_capture(self):
        if self.motion_capture:
            self.motion_capture.stop()
            self.motion_capture = None
            self.start_motion_button.config(state="normal")
            self.stop_motion_button.config(state="disabled")
            self.log("Motion capture stopped.")
            self.instruction_label.config(text="Please start motion capture", fg="red")
            self.start_camera()  # Restart the camera input

    def update_frame(self):
        if self.cap and self.cap.isOpened():
            ret, frame = self.cap.read()
            if ret:
                frame = cv2.flip(frame, 1)  # 水平翻转图像
                frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                img = Image.fromarray(frame)

                # Resize the frame to fit label size while maintaining aspect ratio
                label_width = self.video_label.winfo_width()
                label_height = self.video_label.winfo_height()
                frame_width, frame_height = img.size

                label_ratio = label_width / label_height
                frame_ratio = frame_width / frame_height

                if label_ratio > frame_ratio:
                    new_height = label_height
                    new_width = int(new_height * frame_ratio)
                else:
                    new_width = label_width
                    new_height = int(new_width / frame_ratio)

                img = img.resize((new_width, new_height), Image.LANCZOS)
                imgtk = ImageTk.PhotoImage(image=img)
                self.video_label.imgtk = imgtk
                self.video_label.configure(image=imgtk)
            self.root.after(10, self.update_frame)

    def on_closing(self):
        self.stop_camera()  # Ensure camera is released
        self.root.destroy()

    def monitor_output(self):
        while True:
            try:
                sys.stdout.flush()
                sys.stderr.flush()
                time.sleep(0.1)

                # Check each line in the log queue
                while not self.log_queue.empty():
                    line = self.log_queue.get_nowait().strip()
                    if line:
                        self.detect_log_pattern(line)  # Call pattern detection

            except (SystemExit, KeyboardInterrupt):
                break

    def detect_log_pattern(self, line):
        try:
            # Convert log line to a list of floats
            values = [float(i) for i in line.strip('[]').split()]

            if len(values) >= 6:
                # Check if second element is 99
                if values[1] == 99.0:
                    self.update_instruction_label(
                        "Please keep a one-meter distance, torso and thighs visible, hands on chest (ENG)",
                        "red"
                    )
                else:
                    # Set flag and update instruction label for correct position
                    self.correct_position_flag = True
                    self.update_instruction_label(
                        "Current position is appropriate. Prepare to start the game (ENG)",
                        "green"
                    )
                    self.start_game_button.config(state="normal")

        except ValueError:
            # In case of parsing error, ignore the line
            pass

    # Function to update instruction label text and color
    def update_instruction_label(self, text, color):
        self.instruction_label.config(text=text, fg=color)
    def log(self, message):
        self.log_queue.put(message)

    def update_log(self):
        while not self.log_queue.empty():
            message = self.log_queue.get_nowait()
            self.log_text.config(state="normal")
            self.log_text.insert(tk.END, message + "\n")
            self.log_text.config(state="disabled")
            self.log_text.yview(tk.END)
        self.root.after(100, self.update_log)

    def toggle_collapse(self):
        """Toggle the visibility of the collapsible frame."""
        if self.collapse_frame.winfo_viewable():
            self.collapse_frame.grid_remove()
            self.toggle_button.config(text="▼ Expand")
        else:
            self.collapse_frame.grid(row=2, column=0, columnspan=2, sticky="nsew")
            self.toggle_button.config(text="▲ Collapse")

    def select_game_path(self):
        """Open file dialog to select the game path."""
        file_path = filedialog.askopenfilename(
            title="Select Game File",
        )
        if file_path:
            self.game_path = file_path
            self.path_label.config(text=self.game_path)  # Update the label text
            self.log(f"Game path updated to: {self.game_path}")

    def start_game(self):
        """Attempt to start the game using the specified game path."""
        # 检查路径是否为 .app，并自动追加可执行文件路径
        if self.game_path.endswith(".app"):
            self.game_path = os.path.join(self.game_path, "Contents", "MacOS", "CyberGod")

        # 打印试图执行的完整路径
        print(f"Attempting to execute game at: {self.game_path}")

        # 检查文件是否存在
        if os.path.exists(self.game_path):
            try:
                subprocess.Popen(self.game_path, shell=True)
                self.log("Game started successfully.")
            except Exception as e:
                self.log(f"Failed to start game: {e}")
                messagebox.showerror("Error", "Could not start the game. Please check the game path.")
        else:
            self.log("File not found. Please select the correct game path manually.")
            messagebox.showwarning("File Not Found", "File not found. Please select the correct game path manually.")

if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()

