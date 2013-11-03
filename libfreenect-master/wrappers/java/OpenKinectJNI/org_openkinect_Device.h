/* DO NOT EDIT THIS FILE - it is machine generated */
#include <jni.h>
/* Header for class org_openkinect_Device */

#ifndef _Included_org_openkinect_Device
#define _Included_org_openkinect_Device
#ifdef __cplusplus
extern "C" {
#endif
/*
 * Class:     org_openkinect_Device
 * Method:    jniClose
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniClose
  (JNIEnv *, jobject);

/*
 * Class:     org_openkinect_Device
 * Method:    jniSetLED
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniSetLED
  (JNIEnv *, jobject, jint);

/*
 * Class:     org_openkinect_Device
 * Method:    jniSetTiltDegs
 * Signature: (F)V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniSetTiltDegs
  (JNIEnv *, jobject, jfloat);

/*
 * Class:     org_openkinect_Device
 * Method:    jniSetFormatRGB
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniSetFormatRGB
  (JNIEnv *, jobject, jint);

/*
 * Class:     org_openkinect_Device
 * Method:    jniSetFormatDepth
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniSetFormatDepth
  (JNIEnv *, jobject, jint);

/*
 * Class:     org_openkinect_Device
 * Method:    jniStartCaptureRGB
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniStartCaptureRGB
  (JNIEnv *, jobject);

/*
 * Class:     org_openkinect_Device
 * Method:    jniStartCaptureDepth
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniStartCaptureDepth
  (JNIEnv *, jobject);

/*
 * Class:     org_openkinect_Device
 * Method:    jniStopCaptureRGB
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniStopCaptureRGB
  (JNIEnv *, jobject);

/*
 * Class:     org_openkinect_Device
 * Method:    jniStopCaptureDepth
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_org_openkinect_Device_jniStopCaptureDepth
  (JNIEnv *, jobject);

#ifdef __cplusplus
}
#endif
#endif
