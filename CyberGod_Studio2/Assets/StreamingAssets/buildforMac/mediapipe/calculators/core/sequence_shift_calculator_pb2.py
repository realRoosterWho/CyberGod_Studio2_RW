# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/calculators/core/sequence_shift_calculator.proto
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


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n:mediapipe/calculators/core/sequence_shift_calculator.proto\x12\tmediapipe\x1a$mediapipe/framework/calculator.proto\"\xcb\x01\n\x1eSequenceShiftCalculatorOptions\x12\x19\n\rpacket_offset\x18\x01 \x01(\x05:\x02-1\x12\x35\n&emit_empty_packets_before_first_packet\x18\x02 \x01(\x08:\x05\x66\x61lse2W\n\x03\x65xt\x12\x1c.mediapipe.CalculatorOptions\x18\x87\xba\xa9\x33 \x01(\x0b\x32).mediapipe.SequenceShiftCalculatorOptions')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.calculators.core.sequence_shift_calculator_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:
  mediapipe_dot_framework_dot_calculator__options__pb2.CalculatorOptions.RegisterExtension(_SEQUENCESHIFTCALCULATOROPTIONS.extensions_by_name['ext'])

  DESCRIPTOR._options = None
  _SEQUENCESHIFTCALCULATOROPTIONS._serialized_start=112
  _SEQUENCESHIFTCALCULATOROPTIONS._serialized_end=315
# @@protoc_insertion_point(module_scope)