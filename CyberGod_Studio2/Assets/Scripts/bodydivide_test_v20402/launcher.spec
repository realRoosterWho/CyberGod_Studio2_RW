# -*- mode: python ; coding: utf-8 -*-


a = Analysis(
    ['D:\\Unity\\Cybergod_win_local\\CyberGod_Studio2_RW\\CyberGod_Studio2\\Assets\\Scripts\\bodydivide_test_v20402\\launcher.py'],
    pathex=[],
    binaries=[],
    datas=[('Logo_cyberspirit.png', '.'), ('F:\\anaconda3_f\\envs\\brandnewCyberGod\\Lib\\site-packages\\mediapipe', 'mediapipe'), ('Info.plist', 'Contents/Info.plist')],
    hiddenimports=['cvzone', 'mediapipe', 'shapely._geos'],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
    optimize=0,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
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
