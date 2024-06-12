# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: mediapipe/calculators/util/landmarks_transformation_calculator.proto
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


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\nDmediapipe/calculators/util/landmarks_transformation_calculator.proto\x12\tmediapipe\x1a$mediapipe/framework/calculator.proto\"\xb6\x04\n(LandmarksTransformationCalculatorOptions\x12Z\n\x0etransformation\x18\x01 \x03(\x0b\x32\x42.mediapipe.LandmarksTransformationCalculatorOptions.Transformation\x1a\x16\n\x14NormalizeTranslation\x1aO\n\x08\x46lipAxis\x12\x15\n\x06\x66lip_x\x18\x01 \x01(\x08:\x05\x66\x61lse\x12\x15\n\x06\x66lip_y\x18\x02 \x01(\x08:\x05\x66\x61lse\x12\x15\n\x06\x66lip_z\x18\x03 \x01(\x08:\x05\x66\x61lse\x1a\xe0\x01\n\x0eTransformation\x12i\n\x15normalize_translation\x18\x01 \x01(\x0b\x32H.mediapipe.LandmarksTransformationCalculatorOptions.NormalizeTranslationH\x00\x12Q\n\tflip_axis\x18\x02 \x01(\x0b\x32<.mediapipe.LandmarksTransformationCalculatorOptions.FlipAxisH\x00\x42\x10\n\x0etransformation2b\n\x03\x65xt\x12\x1c.mediapipe.CalculatorOptions\x18\xe8\xdb\xf2\xc8\x01 \x01(\x0b\x32\x33.mediapipe.LandmarksTransformationCalculatorOptions')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'mediapipe.calculators.util.landmarks_transformation_calculator_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:
  mediapipe_dot_framework_dot_calculator__options__pb2.CalculatorOptions.RegisterExtension(_LANDMARKSTRANSFORMATIONCALCULATOROPTIONS.extensions_by_name['ext'])

  DESCRIPTOR._options = None
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS._serialized_start=122
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS._serialized_end=688
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_NORMALIZETRANSLATION._serialized_start=258
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_NORMALIZETRANSLATION._serialized_end=280
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_FLIPAXIS._serialized_start=282
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_FLIPAXIS._serialized_end=361
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_TRANSFORMATION._serialized_start=364
  _LANDMARKSTRANSFORMATIONCALCULATOROPTIONS_TRANSFORMATION._serialized_end=588
# @@protoc_insertion_point(module_scope)
