# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/calculators/core/flow_limiter_calculator.proto
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


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n8mediapipe/calculators/core/flow_limiter_calculator.proto\x12\tmediapipe\x1a$mediapipe/framework/calculator.proto\"\xc7\x01\n\x1c\x46lowLimiterCalculatorOptions\x12\x18\n\rmax_in_flight\x18\x01 \x01(\x05:\x01\x31\x12\x17\n\x0cmax_in_queue\x18\x02 \x01(\x05:\x01\x30\x12\x1c\n\x11in_flight_timeout\x18\x03 \x01(\x03:\x01\x30\x32V\n\x03\x65xt\x12\x1c.mediapipe.CalculatorOptions\x18\xf8\xa0\xf4\x9b\x01 \x01(\x0b\x32\'.mediapipe.FlowLimiterCalculatorOptionsBC\n%com.google.mediapipe.calculator.protoB\x1a\x46lowLimiterCalculatorProto')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.calculators.core.flow_limiter_calculator_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:
  mediapipe_dot_framework_dot_calculator__options__pb2.CalculatorOptions.RegisterExtension(_FLOWLIMITERCALCULATOROPTIONS.extensions_by_name['ext'])

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\n%com.google.mediapipe.calculator.protoB\032FlowLimiterCalculatorProto'
  _FLOWLIMITERCALCULATOROPTIONS._serialized_start=110
  _FLOWLIMITERCALCULATOROPTIONS._serialized_end=309
# @@protoc_insertion_point(module_scope)
