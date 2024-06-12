# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/util/tracking/motion_saliency.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n-mediapipe/util/tracking/motion_saliency.proto\x12\tmediapipe\"\xa5\x04\n\x15MotionSaliencyOptions\x12\x17\n\nbound_left\x18\x01 \x01(\x02:\x03\x30.3\x12\x19\n\x0c\x62ound_bottom\x18\x02 \x01(\x02:\x03\x30.3\x12\x18\n\x0b\x62ound_right\x18\x0f \x01(\x02:\x03\x30.3\x12\x16\n\tbound_top\x18\x10 \x01(\x02:\x03\x30.3\x12\x1b\n\x0fsaliency_weight\x18\x03 \x01(\x02:\x02\x32\x30\x12-\n\x1escale_weight_by_flow_magnitude\x18\x08 \x01(\x08:\x05\x66\x61lse\x12\x17\n\x0cmin_features\x18\x04 \x01(\x05:\x01\x35\x12*\n\x1buse_only_foreground_regions\x18\t \x01(\x08:\x05\x66\x61lse\x12 \n\x14min_irls_mode_weight\x18\n \x01(\x02:\x02\x31\x30\x12\x1d\n\x12num_top_irls_modes\x18\x0b \x01(\x05:\x01\x33\x12\x1c\n\x0fmode_band_width\x18\x0c \x01(\x02:\x03\x30.1\x12!\n\x16selection_frame_radius\x18\x05 \x01(\x05:\x01\x35\x12\'\n\x1aselection_support_distance\x18\x06 \x01(\x02:\x03\x30.2\x12$\n\x19selection_minimum_support\x18\x07 \x01(\x05:\x01\x34\x12#\n\x15\x66iltering_sigma_space\x18\r \x01(\x02:\x04\x30.05\x12\x1f\n\x14\x66iltering_sigma_time\x18\x0e \x01(\x02:\x01\x35')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.util.tracking.motion_saliency_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  _MOTIONSALIENCYOPTIONS._serialized_start=61
  _MOTIONSALIENCYOPTIONS._serialized_end=610
# @@protoc_insertion_point(module_scope)
