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
import platform



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
        self.step4_completed = False  # 添加新的标志来追踪第四步是否完成
        self.motion_capture_active = False  # 添加新的标志来追踪 Motion Capture 状态

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
        initial_width = 500  # Increase the width
        initial_height = 850  # Increase the height
        self.root.geometry(f"{initial_width}x{initial_height}")
        self.root.minsize(initial_width, initial_height)
        self.root.resizable(False, True)  # 添加这行，禁止调整窗口大小

        # Configure grid layout
        self.root.columnconfigure(0, weight=1)
        self.root.columnconfigure(1, weight=1) #这是用来设置第一列和第二列的宽度比例

        self.root.rowconfigure(11, weight=1)

        # Instruction label
        self.instruction_label = tk.Label(
            root, 
            text="", 
            font=("Helvetica", 20, "bold"), 
            fg="red",
            wraplength=450,
            justify="center",
            height=2,  # 设置固定高度为2行
            pady=5    # 增加上下内边距
        )
        self.instruction_label.grid(
            row=0, 
            column=0, 
            columnspan=2, 
            padx=5, 
            pady=5, 
            sticky="nsew"
        )
        self.instruction_label.grid(row=0, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # 使用 update_instruction_label 设置初始文本
        self.update_instruction_label("Step 1: Please select camera from the dropdown list", "red")

        # 在instruction_label后添加新的教程框架
        self.tutorial_frame = ttk.LabelFrame(root, text ="Step by Step Tutorial", padding=(5, 5))
        style = ttk.Style()
        style.configure('TLabelframe', padding=(10, 5))  # 添加整体padding
        style.configure('TLabelframe.Label', 
            font=('Helvetica', 12, 'bold'),
            anchor='center',  # 设置锚点为居中
            justify='center'  # 设置文本对齐为居中
        )
        self.tutorial_frame.grid(row=1, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
        
        # 创建一个Canvas和Scrollbar
        self.canvas = tk.Canvas(self.tutorial_frame, height=120)
        self.scrollbar = ttk.Scrollbar(self.tutorial_frame, orient="vertical", command=self.canvas.yview)
        self.scrollable_frame = ttk.Frame(self.canvas)
        
        self.canvas.configure(yscrollcommand=self.scrollbar.set)
        
        # 绑定滚动事件
        self.scrollable_frame.bind(
            "<Configure>",
            lambda e: self.canvas.configure(scrollregion=self.canvas.bbox("all"))
        )
        
        self.canvas.create_window((0, 0), window=self.scrollable_frame, anchor="nw")
        
        self.canvas.pack(side="left", fill="x", expand=True)
        self.scrollbar.pack(side="right", fill="y")
        
        # 教程步骤列表
        self.tutorial_steps = [
            "Step 1: Select your camera from the dropdown list",
            "Step 2: Click 'Start Camera' to initialize camera",
            "Step 3: Click 'Start Motion Capture' to begin tracking",
            "Step 4: Stand in the correct position (1 meter from camera, torso and thighs visible)",
            "Step 5: Once position is confirmed, click 'Start Game'",
            "Note: If game path is not found, use the expand button below to manually select the game path"
        ]
        
        # 创建步骤标签并存储它们
        self.step_labels = []
        for i, step in enumerate(self.tutorial_steps):
            if "Note:" in step:  # 于注释使用不同的样式
                label = tk.Label(
                    self.scrollable_frame,
                    text=step,
                    fg="red",
                    wraplength=450,
                    justify="left",
                    padx=2,
                    pady=1,
                    anchor="w",
                    font=("Helvetica", 10, "italic")  # 注释使用斜体
                )
            else:  # 对于普通步骤
                label = tk.Label(
                    self.scrollable_frame,
                    text=step,
                    fg="red",
                    wraplength=450,
                    justify="left",
                    padx=2,
                    pady=1,
                    anchor="w",
                    font=("Helvetica", 10, "bold")  # 步骤使用粗体
                )
            label.pack(fill="x", padx=2, pady=1, anchor="w")
            self.step_labels.append(label)

        # Start Game Button and Status Label
        if platform.system() == 'Darwin':  # macOS
            from tkmacosx import Button
            self.start_game_button = Button(
                root, 
                text="Start Game",
                bg="#2E8B57",
                fg="white",
                state="disabled",
                command=self.start_game,
                borderless=1
            )
        else:  # Windows 或其他操作系统
            self.start_game_button = Button(
                root, 
                text="Start Game",
                bg="#2E8B57",
                fg="white",
                state="disabled",
                command=self.start_game
            )
        self.start_game_button.grid(row=7, column=0, padx=5, pady=5, sticky="e")

        # 移动 game_path_frame
        self.game_path_frame = tk.Frame(root)
        self.game_path_frame.grid(row=7, column=1, padx=5, pady=5, sticky="w")

        # 修改游戏状态标签
        self.game_status_label = tk.Label(
            self.game_path_frame, 
            text="",
            font=("Helvetica", 10, "bold")
        )
        self.game_status_label.grid(row=0, column=1, padx=5, pady=5, sticky="w")

        # Toggle button to show/hide the collapsible frame
        if platform.system() == 'Darwin':  # macOS
            from tkmacosx import Button
            # Toggle/Expand Button
            self.toggle_button = Button(
                self.game_path_frame,
                text="▼ Select game path manually",
                command=self.toggle_collapse,
                bg='#666666',  # 深灰色
                fg='white',
                borderless=1,
                width=200  # 为 tkmacosx.Button 设置像素宽度
            )
            self.toggle_button.grid(row=0, column=0, padx=5, pady=5, sticky="w")

        else:  # Windows 或其他操作系统
            # Toggle/Expand Button
            self.toggle_button = tk.Button(
                self.game_path_frame,
                text="▼ Select game path manually",
                command=self.toggle_collapse,
                bg='#666666',
                fg='white',
                font=("Helvetica", 10),
                width=200  # 为 tk.Button 设置字符宽度
            )
            self.toggle_button.grid(row=0, column=0, padx=5, pady=5, sticky="w")

        # Collapsible frame for manual path selection
        self.collapse_frame = tk.Frame(root)
        self.collapse_frame.grid(row=8, column=0, columnspan=2, sticky="nsew")
        self.collapse_frame.grid_remove()  # Start collapsed

        # 修改路径选择标签
        self.path_instruction_label = tk.Label(
            self.collapse_frame,
            text="Please select the file path. \nThe file should be named CyberSpirit.app or CyberSpirit.exe",
            font=("Helvetica", 10, "bold"),
            anchor="w",
            justify="left",
        )
        self.path_instruction_label.grid(row=0, column=0, columnspan=2, sticky="w", padx=5, pady=5)

        # Select Path button with better styling
        if platform.system() == 'Darwin':  # macOS
            from tkmacosx import Button
            self.select_path_button = Button(
                self.collapse_frame,
                text="Select Path",
                command=self.select_game_path,
                bg='white',
                fg='black',
                borderless=1,
                width=100  # For tkmacosx.Button, width is in pixels
            )
        else:  # Windows or other systems
            self.select_path_button = tk.Button(
                self.collapse_frame,
                text="Select Path",
                command=self.select_game_path,
                bg='#007AFF',
                fg='white',
                width=10,  # For tk.Button, width is in characters
                font=('Helvetica', 10)
            )
        self.select_path_button.grid(row=1, column=0, padx=5, pady=5, sticky="e")

        # Create a label to display the path
        self.path_label = tk.Label(
            self.collapse_frame, 
            text=self.game_path, 
            font=("Helvetica", 10), 
            bg = "gray",
            anchor="w",  # 文本左对齐
            justify="left",  # 多行文本左对齐
            wraplength=300
        )
        self.path_label.grid(row=1, column=1, padx=5, pady=5, sticky="w")

        # Log title
        self.log_title = tk.Label(root, text="Log", font=("Helvetica", 12))
        self.log_title.grid(row=9, column=0, columnspan=2, padx=5, pady=(10,0), sticky="nsew")

        # Log text box
        self.log_text = tk.Text(root, height=5, state="normal")
        self.log_text.grid(row=10, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")

        # Video display label
        self.video_label = tk.Label(root)
        self.video_label.grid(row=11, column=0, columnspan=2, sticky="nsew")
        self.root.rowconfigure(11, weight=1)  # 让视频区域可以扩展

        # Load the image
        try:
            if getattr(sys, 'frozen', False):
                # 如果是打包后的程序
                base_path = sys._MEIPASS
            else:
                # 如果是开发环境，使用当前文件所在目录
                base_path = os.path.dirname(os.path.abspath(__file__))
            
            # 修改 logo 文件路径，确保它与 launcher.py 在同一目录
            logo_path = os.path.join(base_path, "Logo_cyberspirit.png")
            print(f"Attempting to load logo from: {logo_path}")  # 调试用
            
            self.logo_image = Image.open(logo_path)
            self.logo_image = self.logo_image.resize((100, 100), Image.LANCZOS)
            self.logo_photo = ImageTk.PhotoImage(self.logo_image)

            self.logo_label = tk.Label(root, image=self.logo_photo)
            self.logo_label.grid(row=4, column=0, padx=5, pady=5, sticky="e")
        except Exception as e:
            self.log(f"Logo loading error: {str(e)}")
            # 如果加载失败，创建一个空的标签
            self.logo_label = tk.Label(root, width=10, height=5)
            self.logo_label.grid(row=4, column=0, padx=5, pady=5, sticky="nsew")

        self.camera_label = tk.Label(root, text="Select Camera:")
        self.camera_label.grid(row=4, column=1, padx=5, pady=5, sticky="w")

        self.camera_listbox = tk.Listbox(root, selectmode=tk.SINGLE, height=5)
        self.camera_listbox.grid(row=4, column=1, padx=5, pady=5, sticky="w")

        # 绑定选择事件
        self.camera_listbox.bind('<<ListboxSelect>>', self.on_camera_select)

        # 填充摄像头列表
        for camera in self.get_camera_list():
            self.camera_listbox.insert(tk.END, camera)

        if platform.system() == 'Darwin':  # macOS
            from tkmacosx import Button
            # Start Motion Button
            self.start_motion_button = Button(
                root,
                text="Start Motion Capture",
                bg="#2E8B57",  # 绿色
                fg="white",
                borderless=1, # 去掉按钮边框
                state="disabled",
                command=self.start_motion_capture
            )
            self.start_motion_button.grid(row=6, column=0, padx=5, pady=5, sticky="e")

            # Stop Motion Button
            self.stop_motion_button = Button(
                root,
                text="Stop Motion Capture",
                bg="#CD5C5C",  # 红色
                fg="white",
                borderless=1,
                state="disabled",
                command=self.stop_motion_capture
            )
            self.stop_motion_button.grid(row=6, column=1, padx=5, pady=5, sticky="w")

            # Start Camera Button
            self.start_button = Button(
                root,
                text="Start Camera",
                bg="#2E8B57",
                fg="white",
                borderless=1,
                command=self.start_camera  # 直接引用方法，不使用 lambda
            )
            # 同样的方式修改其他按钮
            self.stop_button = Button(
                root,
                text="Stop Camera",
                bg="#CD5C5C",
                fg="white",
                borderless=1,
                state="disabled",
                command=self.stop_camera
            )
            self.start_game_button = Button(
                root,
                text="Start Game",
                bg="#2E8B57",
                fg="white",
                state="disabled",
                command=self.start_game,
                borderless=1
            )
        else:  # Windows or other systems
            # Start Motion Button
            self.start_motion_button = tk.Button(
                root,
                text="Start Motion Capture",
                bg="#2E8B57",
                fg="white",
                borderless=1, # 去掉按钮边框
                state="disabled",
                command=self.start_motion_capture,
                font=("Helvetica", 10)
            )
            self.start_motion_button.grid(row=6, column=0, padx=5, pady=5, sticky="e")

            # Stop Motion Button
            self.stop_motion_button = tk.Button(
                root,
                text="Stop Motion Capture",
                bg="#CD5C5C",
                fg="white",
                borderless=1, # 去掉按钮边框
                state="disabled",
                command=self.stop_motion_capture,
                font=("Helvetica", 10)
            )
            self.stop_motion_button.grid(row=6, column=1, padx=5, pady=5, sticky="w")

            # Start Camera Button
            self.start_button = tk.Button(root, text="Start Camera", command=self.start_camera)
            self.stop_button = tk.Button(root, text="Stop Camera", command=self.stop_camera, state="disabled")
            self.start_game_button = tk.Button(root, text="Start Game", command=self.start_game, state="disabled")

        self.start_button.grid(row=5, column=0, padx=5, pady=5, sticky="e")
        self.stop_button.grid(row=5, column=1, padx=5, pady=5, sticky="w")
        self.start_game_button.grid(row=7, column=0, padx=5, pady=5, sticky="e")

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
                self.log(camera_name)
            cap.release()
            index += 1
            
        # 如有像头，自动选中第一个
        if arr:
            self.camera_listbox.select_set(0)  # 选中第一项
            self.camera_listbox.event_generate("<<ListboxSelect>>")  # 触发选择事件
            
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
                self.update_step_status(1, True)  # 完成第二步
                self.instruction_label.config(text="Step 3: Please start Motion Capture", fg="red")
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
            self.instruction_label.config(text="Step 1: Please select camera from the dropdown list", fg="red")

    # 添加新的批量日志方法
    def log_multiple(self, messages):
        """一次性记录多条日志"""
        for message in messages:
            self.log_queue.put(message)
            self.root.update_idletasks()  # 强制更新UI

    # 修改 start_motion_capture 方法中的日志记录
    def start_motion_capture(self):
        if self.cap and not self.motion_capture:
            self.stop_camera()
            selected_camera_index = self.camera_listbox.curselection()
            if selected_camera_index:
                selected_camera = self.camera_listbox.get(selected_camera_index)
                camera_index = int(selected_camera.split()[1])
                self.motion_capture = MotionCapture(camera_index, frame_callback=self.update_motion_frame)

                threading.Thread(target=self.motion_capture.start, daemon=True).start()
                self.start_motion_button.config(state="disabled")
                self.start_button.config(state="disabled")
                self.stop_motion_button.config(state="normal")
                self.motion_capture_active = True
                
                # 使用批量日志
                self.log_multiple([
                    "Motion capture started.",
                    "Please stand in the correct position.",
                    "Waiting for position confirmation..."
                ])
                
                self.update_step_status(2, True)
                self.instruction_label.config(text="Step 4: Please adjust your position", fg="red")
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

    # 修改 stop_motion_capture 方法中的日志记录
    def stop_motion_capture(self):
        if self.motion_capture:
            self.motion_capture.stop()
            self.motion_capture = None
            self.motion_capture_active = False
            self.start_motion_button.config(state="normal")
            self.stop_motion_button.config(state="disabled")
            self.start_button.config(state="normal")
            self.start_game_button.config(state="disabled")
            time.sleep(0.1)
            
            # 使用批量日志
            self.log_multiple([
                "Motion capture stopped.",
                "Camera restarting...",
                "______________________"
            ])
            
            self.update_instruction_label("Step 3: Please Start Motion Capture", "red")
            self.start_camera()
    def update_frame(self):
        if self.cap and self.cap.isOpened():
            ret, frame = self.cap.read()
            if ret:
                # 获取摄像头原始分辨率
                cam_width = self.cap.get(cv2.CAP_PROP_FRAME_WIDTH)
                cam_height = self.cap.get(cv2.CAP_PROP_FRAME_HEIGHT)
                print(f"摄像头分辨率: {cam_width}x{cam_height}")

                frame = cv2.flip(frame, 1)  # 水平翻转图像
                frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                img = Image.fromarray(frame)

                # 获取显示标签的大小
                label_width = self.video_label.winfo_width()
                label_height = self.video_label.winfo_height()
                print(f"显示标签大小: {label_width}x{label_height}")

                # 获取当前帧的大小
                frame_width, frame_height = img.size
                print(f"当前帧大小: {frame_width}x{frame_height}")

                # 计算缩放比例
                label_ratio = label_width / label_height
                frame_ratio = frame_width / frame_height

                if label_ratio > frame_ratio:
                    new_height = label_height
                    new_width = int(new_height * frame_ratio)
                else:
                    new_width = label_width
                    new_height = int(new_width / frame_ratio)

                print(f"缩放后大小: {new_width}x{new_height}")

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
        # 如果 Motion Capture 未激活，直接返回
        if not self.motion_capture_active:
            return
        
        try:
            values = [float(i) for i in line.strip('[]').split()]
            if len(values) >= 6:
                if not self.step4_completed:
                    if values[1] == 99.0:
                        self.update_instruction_label("Step 4: Please adjust your position", "red")
                    else:
                        self.step4_completed = True
                        self.update_step_status(3, True)
                        self.correct_position_flag = True
                        self.start_game_button.config(state="normal")
                
                if values[1] == 99.0:
                    self.update_instruction_label("Step 4: Please adjust your position", "red")
                else:
                    self.start_game_button.config(state="normal")
                    self.update_instruction_label("Step 4: Current position is appropriate. Prepare to start the game", "green")
        except ValueError:
            pass

    # Function to update instruction label text and color
    def update_instruction_label(self, text, color="red"):
        # 根据不同的文本内容设置对应的步骤提示
        if "position" in text.lower():
            if "appropriate" in text.lower():
                display_text = "Step 4: Current position is appropriate. Prepare to start the game"
            else:
                display_text = "Step 4: Please stand in the correct position"
        elif "please open motion capture" in text.lower():
            display_text = "Step 3: Please Start Motion Capture"
        elif "please select camera" in text.lower():
            display_text = "Step 1: Please select camera from the dropdown list"
        elif "initializing camera" in text.lower():
            display_text = "Step 2: Initializing Camera"
        else:
            display_text = text

        self.instruction_label.config(text=display_text, fg=color)

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
        if platform.system() == 'Darwin':  # macOS
            expand_text = "▼ Select game path manually"
            collapse_text = "▲ Collapse"
            # tkmacosx.Button 的配置方式
            if self.collapse_frame.winfo_viewable():
                self.collapse_frame.grid_remove()
                self.toggle_button.configure(
                    text=expand_text,
                    bg='#666666',
                    fg='white',
                    width=200
                )
            else:
                self.collapse_frame.grid(row=8, column=0, columnspan=2, sticky="nsew")
                self.toggle_button.configure(
                    text=collapse_text,
                    bg='#666666',
                    fg='white',
                    width=200
                )
        else:  # Windows 或其他操作系统
            # 标准 tk.Button 的配置方式
            if self.collapse_frame.winfo_viewable():
                self.collapse_frame.grid_remove()
                self.toggle_button.config(
                    text="▼ Select game path manually",
                    font=("Helvetica", 10),
                    width=25
                )
            else:
                self.collapse_frame.grid(row=8, column=0, columnspan=2, sticky="nsew")
                self.toggle_button.config(
                    text="▲ Collapse",
                    font=("Helvetica", 10),
                    width=25
                )

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
                self.update_step_status(4, True)  # 完成第五步
                self.log("Game started successfully.")
            except Exception as e:
                self.update_step_status(4, False)
                self.log(f"Failed to start game: {e}")
                messagebox.showerror("Error", "Could not start the game. Please check the game path.")
        else:
            self.log("File not found. Please select the correct game path manually.")
            messagebox.showwarning("File Not Found", "File not found. Please select the correct game path manually.")

    # 添加更新步骤状态的方法
    def update_step_status(self, step_index, completed=True):
        if 0 <= step_index < len(self.step_labels):
            self.step_labels[step_index].config(fg="green" if completed else "red")

    # 添加新的事件处理方法
    def on_camera_select(self, event):
        if self.camera_listbox.curselection():  # 如果有选中项
            self.update_step_status(0, True)  # 更新第一步状态为完成
            self.update_instruction_label("Step 2: Please start camera", "red")


        # else:
        #     self.update_step_status(0, False)  # 如果没有选中项，将状态设为未完成

if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()

