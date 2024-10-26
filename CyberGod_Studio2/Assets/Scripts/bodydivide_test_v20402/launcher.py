import tkinter as tk
from tkinter import ttk
import cv2
from PIL import Image, ImageTk


class CameraApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Camera Selector")

        # Set initial window size
        initial_width = 640
        initial_height = 480
        self.root.geometry(f"{initial_width}x{initial_height}")
        self.root.minsize(initial_width, initial_height)

        # Configure grid layout
        self.root.columnconfigure(0, weight=1)
        self.root.columnconfigure(1, weight=1)
        self.root.rowconfigure(2, weight=1)

        self.camera_label = tk.Label(root, text="Select Camera:")
        self.camera_label.grid(row=0, column=0, padx=5, pady=5, sticky="e")

        self.camera_listbox = tk.Listbox(root, selectmode=tk.SINGLE, height=5)
        self.camera_listbox.grid(row=0, column=1, padx=5, pady=5, sticky="we")
        for camera in self.get_camera_list():
            self.camera_listbox.insert(tk.END, camera)

        self.start_button = tk.Button(root, text="Start Camera", command=self.start_camera)
        self.start_button.grid(row=1, column=0, padx=5, pady=5, sticky="e")

        self.stop_button = tk.Button(root, text="Stop Camera", command=self.stop_camera, state="disabled")
        self.stop_button.grid(row=1, column=1, padx=5, pady=5, sticky="w")

        # Video display label with initial size
        self.video_label = tk.Label(root, width=initial_width, height=initial_height)
        self.video_label.grid(row=2, column=0, columnspan=2, sticky="nsew")

        self.cap = None
        self.root.protocol("WM_DELETE_WINDOW", self.on_closing)

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
                arr.append(f"Camera {index} - {width}x{height}")
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
            else:
                self.cap.release()
                self.cap = None
                print("Failed to open the selected camera.")

    def stop_camera(self):
        if self.cap:
            self.cap.release()
            self.cap = None
            self.video_label.config(image="")
            self.stop_button.config(state="disabled")
            self.start_button.config(state="normal")

    def update_frame(self):
        if self.cap and self.cap.isOpened():
            ret, frame = self.cap.read()
            if ret:
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


if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()