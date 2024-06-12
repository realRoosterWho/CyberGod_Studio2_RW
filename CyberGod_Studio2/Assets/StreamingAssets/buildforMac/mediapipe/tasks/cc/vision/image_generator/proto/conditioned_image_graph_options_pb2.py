# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/tasks/cc/vision/image_generator/proto/conditioned_image_graph_options.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from mediapipe.framework import calculator_pb2 as mediapipe_dot_framework_dot_calculator__pb2
try:
  mediapipe_dot_framework_dot_calculator__options__pb2 = mediapipe_dot_framework_dot_calculator__pb2.mediapipe_dot_framework_dot_calculator__options__pb2
except AttributeError:
  mediapipe_dot_framework_dot_calculator__options__pb2 = mediapipe_dot_framework_dot_calculator__pb2.mediapipe.framework.calculator_options_pb2
from mediapipe.tasks.cc.vision.face_landmarker.proto import face_landmarker_graph_options_pb2 as mediapipe_dot_tasks_dot_cc_dot_vision_dot_face__landmarker_dot_proto_dot_face__landmarker__graph__options__pb2
from mediapipe.tasks.cc.vision.image_segmenter.proto import image_segmenter_graph_options_pb2 as mediapipe_dot_tasks_dot_cc_dot_vision_dot_image__segmenter_dot_proto_dot_image__segmenter__graph__options__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\nUmediapipe/tasks/cc/vision/image_generator/proto/conditioned_image_graph_options.proto\x12,mediapipe.tasks.vision.image_generator.proto\x1a$mediapipe/framework/calculator.proto\x1aSmediapipe/tasks/cc/vision/face_landmarker/proto/face_landmarker_graph_options.proto\x1aSmediapipe/tasks/cc/vision/image_segmenter/proto/image_segmenter_graph_options.proto\"\xf0\x06\n\x1c\x43onditionedImageGraphOptions\x12\x8a\x01\n\x1b\x66\x61\x63\x65_condition_type_options\x18\x02 \x01(\x0b\x32\x63.mediapipe.tasks.vision.image_generator.proto.ConditionedImageGraphOptions.FaceConditionTypeOptionsH\x00\x12\x8a\x01\n\x1b\x65\x64ge_condition_type_options\x18\x03 \x01(\x0b\x32\x63.mediapipe.tasks.vision.image_generator.proto.ConditionedImageGraphOptions.EdgeConditionTypeOptionsH\x00\x12\x8c\x01\n\x1c\x64\x65pth_condition_type_options\x18\x04 \x01(\x0b\x32\x64.mediapipe.tasks.vision.image_generator.proto.ConditionedImageGraphOptions.DepthConditionTypeOptionsH\x00\x1a\x8b\x01\n\x18\x46\x61\x63\x65\x43onditionTypeOptions\x12o\n\x1d\x66\x61\x63\x65_landmarker_graph_options\x18\x01 \x01(\x0b\x32H.mediapipe.tasks.vision.face_landmarker.proto.FaceLandmarkerGraphOptions\x1ap\n\x18\x45\x64geConditionTypeOptions\x12\x13\n\x0bthreshold_1\x18\x01 \x01(\x02\x12\x13\n\x0bthreshold_2\x18\x02 \x01(\x02\x12\x15\n\raperture_size\x18\x03 \x01(\x05\x12\x13\n\x0bl2_gradient\x18\x04 \x01(\x08\x1a\x8c\x01\n\x19\x44\x65pthConditionTypeOptions\x12o\n\x1dimage_segmenter_graph_options\x18\x01 \x01(\x0b\x32H.mediapipe.tasks.vision.image_segmenter.proto.ImageSegmenterGraphOptionsB\x18\n\x16\x63ondition_type_optionsB[\n6com.google.mediapipe.tasks.vision.imagegenerator.protoB!ConditionedImageGraphOptionsProtob\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.tasks.cc.vision.image_generator.proto.conditioned_image_graph_options_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\n6com.google.mediapipe.tasks.vision.imagegenerator.protoB!ConditionedImageGraphOptionsProto'
  _CONDITIONEDIMAGEGRAPHOPTIONS._serialized_start=344
  _CONDITIONEDIMAGEGRAPHOPTIONS._serialized_end=1224
  _CONDITIONEDIMAGEGRAPHOPTIONS_FACECONDITIONTYPEOPTIONS._serialized_start=802
  _CONDITIONEDIMAGEGRAPHOPTIONS_FACECONDITIONTYPEOPTIONS._serialized_end=941
  _CONDITIONEDIMAGEGRAPHOPTIONS_EDGECONDITIONTYPEOPTIONS._serialized_start=943
  _CONDITIONEDIMAGEGRAPHOPTIONS_EDGECONDITIONTYPEOPTIONS._serialized_end=1055
  _CONDITIONEDIMAGEGRAPHOPTIONS_DEPTHCONDITIONTYPEOPTIONS._serialized_start=1058
  _CONDITIONEDIMAGEGRAPHOPTIONS_DEPTHCONDITIONTYPEOPTIONS._serialized_end=1198
# @@protoc_insertion_point(module_scope)
