import tkinter as tk
from tkinter import ttk
import cv2
from PIL import Image, ImageTk
import threading
import queue
import sys
import os
from motion import MotionCapture  # Import the MotionCapture class
import threading
import time

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
        # Start the output monitoring thread
        self.monitoring_thread = threading.Thread(target=self.monitor_output, daemon=True)
        self.monitoring_thread.start()

        # Set initial window size
        initial_width = 800  # Increase the width
        initial_height = 600  # Increase the height
        self.root.geometry(f"{initial_width}x{initial_height}")
        self.root.minsize(initial_width, initial_height)

        # Configure grid layout
        self.root.columnconfigure(0, weight=1)
        self.root.columnconfigure(1, weight=1)
        self.root.rowconfigure(6, weight=1)
        self.root.rowconfigure(7, weight=1)

        # Instruction label
        self.instruction_label = tk.Label(root, text="Please Select Camera", font=("Helvetica", 16), fg="red")
        self.instruction_label.grid(row=0, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # Load the image
        base_path = getattr(sys, '_MEIPASS', os.path.abspath("."))
        logo_path = os.path.join(base_path, "Logo_cyberspirit.png")
        self.logo_image = Image.open(logo_path)
        self.logo_image = self.logo_image.resize((100, 100), Image.LANCZOS)
        self.logo_photo = ImageTk.PhotoImage(self.logo_image)

        # Create a label for the image
        self.logo_label = tk.Label(root, image=self.logo_photo)
        self.logo_label.grid(row=1, column=0, padx=5, pady=5, sticky="nsew")

        self.camera_label = tk.Label(root, text="Select Camera:")
        self.camera_label.grid(row=1, column=1, padx=5, pady=5, sticky="e")

        self.camera_listbox = tk.Listbox(root, selectmode=tk.SINGLE, height=5)
        self.camera_listbox.grid(row=1, column=1, padx=5, pady=5, sticky="we")

        # Initialize log queue before calling get_camera_list
        self.log_queue = queue.Queue()
        self.log("Application started.")
        for camera in self.get_camera_list():
            self.camera_listbox.insert(tk.END, camera)

        self.start_button = tk.Button(root, text="Start Camera", command=self.start_camera)
        self.start_button.grid(row=2, column=0, padx=5, pady=5, sticky="e")

        self.stop_button = tk.Button(root, text="Stop Camera", command=self.stop_camera, state="disabled")
        self.stop_button.grid(row=2, column=1, padx=5, pady=5, sticky="w")

        self.start_motion_button = tk.Button(root, text="Start Motion Capture", command=self.start_motion_capture, state="disabled")
        self.start_motion_button.grid(row=3, column=0, padx=5, pady=5, sticky="e")

        self.stop_motion_button = tk.Button(root, text="Stop Motion Capture", command=self.stop_motion_capture, state="disabled")
        self.stop_motion_button.grid(row=3, column=1, padx=5, pady=5, sticky="w")

        # Log title
        self.log_title = tk.Label(root, text="Log", font=("Helvetica", 12))
        self.log_title.grid(row=4, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # Log text box
        self.log_text = tk.Text(root, height=5, state="normal")  # Make sure it's editable initially
        self.log_text.grid(row=5, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
        self.log_text.config(state="disabled")  # Set to disabled after initial setup

        # Video display label with initial size
        self.video_label = tk.Label(root, width=initial_width, height=initial_height)
        self.video_label.grid(row=6, column=0, columnspan=2, sticky="nsew")

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
            except (SystemExit, KeyboardInterrupt):
                break

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

if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()