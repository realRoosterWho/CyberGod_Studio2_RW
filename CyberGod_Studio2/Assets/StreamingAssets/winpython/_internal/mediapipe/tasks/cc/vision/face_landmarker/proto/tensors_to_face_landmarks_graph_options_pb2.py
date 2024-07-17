# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/tasks/cc/vision/face_landmarker/proto/tensors_to_face_landmarks_graph_options.proto
"""Generated protocol buffer code."""
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
from google.protobuf.internal import builder as _builder
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from mediapipe.framework import calculator_pb2 as mediapipe_dot_framework_dot_calculator__pb2
try:
  mediapipe_dot_framework_dot_calculator__options__pb2 = mediapipe_dot_framework_dot_calculator__pb2.mediapipe_dot_framework_dot_calculator__options__pb2
except AttributeError:
  mediapipe_dot_framework_dot_calculator__options__pb2 = mediapipe_dot_framework_dot_calculator__pb2.mediapipe.framework.calculator_options_pb2
from mediapipe.framework import calculator_options_pb2 as mediapipe_dot_framework_dot_calculator__options__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n]mediapipe/tasks/cc/vision/face_landmarker/proto/tensors_to_face_landmarks_graph_options.proto\x12,mediapipe.tasks.vision.face_landmarker.proto\x1a$mediapipe/framework/calculator.proto\x1a,mediapipe/framework/calculator_options.proto\"\xdc\x01\n\"TensorsToFaceLandmarksGraphOptions\x12\x19\n\x11input_image_width\x18\x01 \x01(\x05\x12\x1a\n\x12input_image_height\x18\x02 \x01(\x05\x32\x7f\n\x03\x65xt\x12\x1c.mediapipe.CalculatorOptions\x18\x8c\xe8\x80\xf3\x01 \x01(\x0b\x32P.mediapipe.tasks.vision.face_landmarker.proto.TensorsToFaceLandmarksGraphOptions')

_globals = globals()
_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, _globals)
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.tasks.cc.vision.face_landmarker.proto.tensors_to_face_landmarks_graph_options_pb2', _globals)
if _descriptor._USE_C_DESCRIPTORS == False:
  mediapipe_dot_framework_dot_calculator__options__pb2.CalculatorOptions.RegisterExtension(_TENSORSTOFACELANDMARKSGRAPHOPTIONS.extensions_by_name['ext'])

  DESCRIPTOR._options = None
  _globals['_TENSORSTOFACELANDMARKSGRAPHOPTIONS']._serialized_start=228
  _globals['_TENSORSTOFACELANDMARKSGRAPHOPTIONS']._serialized_end=448
# @@protoc_insertion_point(module_scope)