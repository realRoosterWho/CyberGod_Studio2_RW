# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/framework/tool/source.proto
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


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n%mediapipe/framework/tool/source.proto\x12\tmediapipe\x1a$mediapipe/framework/calculator.proto\"\xfa\x02\n%SidePacketsToStreamsCalculatorOptions\x12\x15\n\nnum_inputs\x18\x01 \x01(\x05:\x01\x31\x12\x66\n\rset_timestamp\x18\x02 \x01(\x0e\x32\x41.mediapipe.SidePacketsToStreamsCalculatorOptions.SetTimestampMode:\x0cVECTOR_INDEX\x12 \n\x12vectors_of_packets\x18\x03 \x01(\x08:\x04true\"P\n\x10SetTimestampMode\x12\x10\n\x0cVECTOR_INDEX\x10\x00\x12\x0e\n\nPRE_STREAM\x10\x01\x12\x10\n\x0cWHOLE_STREAM\x10\x02\x12\x08\n\x04NONE\x10\x03\x32^\n\x03\x65xt\x12\x1c.mediapipe.CalculatorOptions\x18\xb7\x8c\x8a\x1d \x01(\x0b\x32\x30.mediapipe.SidePacketsToStreamsCalculatorOptions')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.framework.tool.source_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:
  mediapipe_dot_framework_dot_calculator__options__pb2.CalculatorOptions.RegisterExtension(_SIDEPACKETSTOSTREAMSCALCULATOROPTIONS.extensions_by_name['ext'])

  DESCRIPTOR._options = None
  _SIDEPACKETSTOSTREAMSCALCULATOROPTIONS._serialized_start=91
  _SIDEPACKETSTOSTREAMSCALCULATOROPTIONS._serialized_end=469
  _SIDEPACKETSTOSTREAMSCALCULATOROPTIONS_SETTIMESTAMPMODE._serialized_start=293
  _SIDEPACKETSTOSTREAMSCALCULATOROPTIONS_SETTIMESTAMPMODE._serialized_end=373
# @@protoc_insertion_point(module_scope)
