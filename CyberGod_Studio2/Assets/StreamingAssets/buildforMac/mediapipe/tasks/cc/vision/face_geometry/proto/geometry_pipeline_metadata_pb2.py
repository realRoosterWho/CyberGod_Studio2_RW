# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/tasks/cc/vision/face_geometry/proto/geometry_pipeline_metadata.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from mediapipe.tasks.cc.vision.face_geometry.proto import mesh_3d_pb2 as mediapipe_dot_tasks_dot_cc_dot_vision_dot_face__geometry_dot_proto_dot_mesh__3d__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\nNmediapipe/tasks/cc/vision/face_geometry/proto/geometry_pipeline_metadata.proto\x12*mediapipe.tasks.vision.face_geometry.proto\x1a;mediapipe/tasks/cc/vision/face_geometry/proto/mesh_3d.proto\":\n\x13WeightedLandmarkRef\x12\x13\n\x0blandmark_id\x18\x01 \x01(\r\x12\x0e\n\x06weight\x18\x02 \x01(\x02\"\x99\x02\n\x18GeometryPipelineMetadata\x12M\n\x0cinput_source\x18\x03 \x01(\x0e\x32\x37.mediapipe.tasks.vision.face_geometry.proto.InputSource\x12J\n\x0e\x63\x61nonical_mesh\x18\x01 \x01(\x0b\x32\x32.mediapipe.tasks.vision.face_geometry.proto.Mesh3d\x12\x62\n\x19procrustes_landmark_basis\x18\x02 \x03(\x0b\x32?.mediapipe.tasks.vision.face_geometry.proto.WeightedLandmarkRef*S\n\x0bInputSource\x12\x0b\n\x07\x44\x45\x46\x41ULT\x10\x00\x12\x1a\n\x16\x46\x41\x43\x45_LANDMARK_PIPELINE\x10\x01\x12\x1b\n\x17\x46\x41\x43\x45_DETECTION_PIPELINE\x10\x02\x42U\n4com.google.mediapipe.tasks.vision.facegeometry.protoB\x1dGeometryPipelineMetadataProto')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.tasks.cc.vision.face_geometry.proto.geometry_pipeline_metadata_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\n4com.google.mediapipe.tasks.vision.facegeometry.protoB\035GeometryPipelineMetadataProto'
  _INPUTSOURCE._serialized_start=531
  _INPUTSOURCE._serialized_end=614
  _WEIGHTEDLANDMARKREF._serialized_start=187
  _WEIGHTEDLANDMARKREF._serialized_end=245
  _GEOMETRYPIPELINEMETADATA._serialized_start=248
  _GEOMETRYPIPELINEMETADATA._serialized_end=529
# @@protoc_insertion_point(module_scope)