# -*- mode: python ; coding: utf-8 -*-


block_cipher = None


a = Analysis(
    ['/Volumes/Rooster_SSD/_Unity_Projects/CyberGod_Studio2/CyberGod_Studio2_RW/CyberGod_Studio2/Assets/Scripts/bodydivide_test_v20402/launcher.py'],
    pathex=[],
    binaries=[],
    datas=[('Logo_cyberspirit.png', '.'), ('/Volumes/Rooster_SSD/Anaconda/anaconda3/envs/cybergod/lib/python3.9/site-packages/mediapipe', 'mediapipe')],
    hiddenimports=['cvzone', 'mediapipe', 'shapely._geos'],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
    noarchive=False,
)
pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.zipfiles,
    a.datas,
    [],
    name='launcher',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=True,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)
