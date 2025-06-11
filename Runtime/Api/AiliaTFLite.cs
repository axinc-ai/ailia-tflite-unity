/**
* \~japanese
* @file
* @brief AILIA TFLITE Unity Plugin Native Interface
* @author AXELL Corporation
* @date  November 1, 2022
*/
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace ailiaTFLite{
public class AiliaTFLite
{
    /****************************************************************
    * Tensorのデータ タイプ
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_FLOAT32
    * @brief FLOAT32
    *
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_FLOAT32
    * @brief FLOAT32
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_FLOAT32               =(   0);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_FLOAT16
    * @brief FLOAT16
    *
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_FLOAT16
    * @brief FLOAT16
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_FLOAT16               =(   1);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_INT32
    * @brief INT32
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_INT32
    * @brief INT32
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_INT32                 =(   2);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_UINT8
    * @brief UINT8
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_UINT8
    * @brief UINT8
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_UINT8                 =(   3);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_INT64
    * @brief INT64
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_INT64
    * @brief INT64
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_INT64                 =(   4);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_STRING
    * @brief STRING
    *
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_STRING
    * @brief STRING
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_STRING                =(   5);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_BOOL
    * @brief BOOL
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_BOOL
    * @brief BOOL
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_BOOL                  =(   6);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_INT16
    * @brief INT16
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_INT16
    * @brief INT16
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_INT16                 =(   7);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_COMPLEX64
    * @brief COMPLEX64
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_COMPLEX64
    * @brief COMPLEX64
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_COMPLEX64             =(   8);
    /**
    * \~japanese
    * @def AILIA_TFLITE_TENSOR_TYPE_INT8
    * @brief INT8
    * 
    * \~english
    * @def AILIA_TFLITE_TENSOR_TYPE_INT8
    * @brief INT8
    */
    public const Int32  AILIA_TFLITE_TENSOR_TYPE_INT8                  =(   9);

    /****************************************************************
    * ステータスコード
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_SUCCESS
    * @brief 成功
    *
    * \~english
    * @def AILIA_TFLITE_STATUS_SUCCESS
    * @brief 成功
    */
    public const Int32  AILIA_TFLITE_STATUS_SUCCESS                     =(   0);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_INVALID_ARGUMENT
    * @brief 引数が不正 このステータスコードが返ってきた場合、呼び出し元のコードや渡している引数がNULLになっていないかなどを見直してください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_INVALID_ARGUMENT
    * @brief If the argument is fraudulent, if the status code is returned, review the caller code or the argument passed in NULL.
    */
    public const Int32  AILIA_TFLITE_STATUS_INVALID_ARGUMENT            =(  -1);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_OUT_OF_RANGE
    * @brief 引数などが範囲外 このステータスコードが返ってきた場合、引数が範囲外になっていないかなどを見直してください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_OUT_OF_RANGE
    * @brief If the argument is out of range. returns this status code, please review whether the argument is out of the range.
    */
    public const Int32  AILIA_TFLITE_STATUS_OUT_OF_RANGE                =(  -2);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_MEMORY_INSUFFICIENT
    * @brief メモリが不足している このステータスコードはメモリ確保に失敗した場合などに返ってきます。メモリの空き状況を確認し、ほかのプロセスを終了するなどの対処をしてください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_MEMORY_INSUFFICIENT
    * @brief This status code, which has a shortage of memory, is returned when the memory is secured.Check the availability of the memory and take action, such as terminating other processes.
    */
    public const Int32  AILIA_TFLITE_STATUS_MEMORY_INSUFFICIENT         =(  -3);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_BROKEN_MODEL
    * @brief モデルが破損している このステータスコードは与えられたモデルファイルが破損している場合に返ってきます。モデルファイルが正しいかどうかをご確認ください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_BROKEN_MODEL
    * @brief This status code, which is damaged by the model, is returned when the given model file is damaged.Please check if the model file is correct.
    */
    public const Int32  AILIA_TFLITE_STATUS_BROKEN_MODEL                =(  -4);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_INVALID_PARAMETER
    * @brief モデルのパラメーターが不正 このステータスコードはモデル内に含まれているパラメーターが不正な場合に返ってきます。モデルファイルが正しいかどうかをご確認ください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_INVALID_PARAMETER
    * @brief The model parameter is fraudulent This status code returns when the parameters contained in the model are fraudulent.Please check if the model file is correct.
    */
    public const Int32  AILIA_TFLITE_STATUS_INVALID_PARAMETER           =(  -5);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND
    * @brief パラメーターが存在しない このステータスコードはモデル内で指定したパラメーターが存在しない場合に返ってきます。呼び出し元のコードやモデルファイルが正しいかどうかをご確認ください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND
    * @brief This status code without parameters is returned when the specified parameter does not exist in the model.Please check if the code or model file of the caller is correct.
    */
    public const Int32  AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND         =(  -6);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_UNSUPPORTED_OPCODE
    * @brief 非対応のオペレーターを実行しようとした このステータスコードはailia TFLite runtimeでサポートされていないオペレーターが含まれていた場合に返ってきます。ドキュメントに記載されているオペレーター以外がモデルに含まれていないかご確認ください。オペレーターの新規追加に関しましてはドキュメント記載の問い合わせ先までお問い合わせください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_UNSUPPORTED_OPCODE
    * @brief This status code, which tried to run a non -brief -compatible operator, is returned when the ailia TFLite runtime is not supported.Make sure that the model is not included in the model other than the operator listed in the document.Please contact the documentary contact information for the addition of a new operator.
    */
    public const Int32  AILIA_TFLITE_STATUS_UNSUPPORTED_OPCODE          =(  -7);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_LICENSE_NOT_FOUND
    * @brief 有効なライセンスが見つからない このステータスコードはトライアル版ailia TFLite runtimeを実行している場合で、ライセンスファイルが見つからなかった場合に返ってきます。トライアル版のライセンスに関しましてはドキュメント記載の問い合わせ先までお問い合わせください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_LICENSE_NOT_FOUND
    * @brief This status code is not found in this status code when the trial version ailia TFLite runtime is running and if the license file is not found.For the trial version license, please contact the documentary inquiry.
    */
    public const Int32  AILIA_TFLITE_STATUS_LICENSE_NOT_FOUND           =(  -8);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_LICENSE_BROKEN
    * @brief ライセンスが壊れている このステータスコードはトライアル版ailia TFLite runtimeを実行している場合で、ライセンスファイルが破損している場合に返ってきます。トライアル版のライセンスに関しましてはドキュメント記載の問い合わせ先までお問い合わせください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_LICENSE_BROKEN
    * @brief This status code, which is broken, is returned when the license file is damaged when the trial version ailia TFLite runtime is running.For the trial version license, please contact the documentary inquiry.
    */
    public const Int32  AILIA_TFLITE_STATUS_LICENSE_BROKEN              =(  -9);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_LICENSE_EXPIRED
    * @brief ライセンスの有効期限切れ  このステータスコードはトライアル版ailia TFLite runtimeを実行している場合で、ライセンスファイルの有効期限が切れている場合に返ってきます。トライアル版のライセンスの更新に関しましてはドキュメント記載の問い合わせ先までお問い合わせください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_LICENSE_EXPIRED
    * @brief License expired status code is returned when the license file is expired when the trial version ailia TFLite runtime is running.Please contact the documentary contact information for updating the trial version license.
    */
    public const Int32  AILIA_TFLITE_STATUS_LICENSE_EXPIRED             =( -10);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_INVALID_STATE
    * @brief APIを呼び出せるステートでない このステータスコードが返ってきた場合、APIを呼び出せる状態かどうかなどを見直してください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_INVALID_STATE
    * @brief If this status code, which is not a state that can call the API, returns, review whether it can call the API.
    */
    public const Int32  AILIA_TFLITE_STATUS_INVALID_STATE               =( -11);
    /**
    * \~japanese
    * @def AILIA_TFLITE_STATUS_OTHER_ERROR
    * @brief その他のエラー 上記以外のエラーが発生した場合に返ります。ドキュメント記載の問い合わせ先までお問い合わせください。
    * 
    * \~english
    * @def AILIA_TFLITE_STATUS_OTHER_ERROR
    * @brief Returns when an error other than the above occurs.Please contact us to the contact information described in the documentation.
    */
    public const Int32  AILIA_TFLITE_STATUS_OTHER_ERROR                 =(-128);

    /****************************************************************
    * バックエンドAPI
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_ENV_REFERENCE
    * @brief リファレンスCPU実装
    * 
    * \~english
    * @def AILIA_TFLITE_ENV_REFERENCE
    * @brief Reference CPU implementation
    */
    public const Int32  AILIA_TFLITE_ENV_REFERENCE                 =(   0);
    /**
    * \~japanese
    * @def AILIA_TFLITE_ENV_NNAPI
    * @brief NNAPIを使用した実装。Android向け。
    * 
    * \~english
    * @def AILIA_TFLITE_ENV_NNAPI
    * @brief Implementation using NNAPI.For Android.
    */
    public const Int32  AILIA_TFLITE_ENV_NNAPI                     =(   1);
    /**
    * \~japanese
    * @def AILIA_TFLITE_ENV_MMALIB
    * @brief MMALIBを使用した実装。PCではMMALIBのエミュレータ、デバイスではMMALIBを使用する。
    * 
    * \~english
    * @def AILIA_TFLITE_ENV_MMALIB
    * @brief Implementation using mmalib.The PC uses mmalib emulator, and mmalib is used for the device.
    */
    public const Int32  AILIA_TFLITE_ENV_MMALIB                    =(   2);
    /**
    * \~japanese
    * @def AILIA_TFLITE_ENV_MMALIB_COMPATIBLE
    * @brief MMALIBの出力互換実装。出力一致のみを行うため高速に動作する。
    * 
    * \~english
    * @def AILIA_TFLITE_ENV_MMALIB_COMPATIBLE
    * @brief MMALIB output compatible implementation.It operates at high speed to perform only the output match.
    */
    public const Int32  AILIA_TFLITE_ENV_MMALIB_COMPATIBLE         =(   3);

    /****************************************************************
    * メモリモード
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_MEMORY_MODE_DEFAULT
    * @brief 通常モード
    * 
    * \~english
    * @def AILIA_TFLITE_MEMORY_MODE_DEFAULT
    * @brief Normal mode
    */
    public const Int32  AILIA_TFLITE_MEMORY_MODE_DEFAULT                 =(   0);
    /**
    * \~japanese
    * @def AILIA_TFLITE_MEMORY_MODE_REDUCE_INTERSTAGE
    * @brief 省メモリモード
    * 
    * \~english
    * @def AILIA_TFLITE_MEMORY_MODE_REDUCE_INTERSTAGE
    * @brief Low Memory mode
    */
    public const Int32  AILIA_TFLITE_MEMORY_MODE_REDUCE_INTERSTAGE       =(   1);

    /****************************************************************
    * プロファイルモード
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_PROFILE_MODE_DISABLE
    * @brief プロファイル無効
    * 
    * \~english
    * @def AILIA_TFLITE_PROFILE_MODE_DISABLE
    * @brief Profile disabled
    */
    public const Int32  AILIA_TFLITE_PROFILE_MODE_DISABLE                =(   0);
    /**
    * \~japanese
    * @def AILIA_TFLITE_PROFILE_MODE_ENABLE
    * @brief プロファイル有効
    * 
    * \~english
    * @def AILIA_TFLITE_PROFILE_MODE_ENABLE
    * @brief Profile enabled
    */
    public const Int32  AILIA_TFLITE_PROFILE_MODE_ENABLE                 =(   1);
    /**
    * \~japanese
    * @def AILIA_TFLITE_PROFILE_MODE_TRACE
    * @brief トレース有効
    * 
    * \~english
    * @def AILIA_TFLITE_PROFILE_MODE_TRACE
    * @brief Trace enabled
    */
    public const Int32  AILIA_TFLITE_PROFILE_MODE_TRACE                  =(   2);

  /**
    * \~japanese
    * @def AILIA_TFLITE_PROFILE_MODE_MEMORY
    * @brief メモリプロファイル有効
    *
    * \~english
    * @def AILIA_TFLITE_PROFILE_MODE_MEMORY
    * @brief Memory profile enabled
    */
    public const Int32  AILIA_TFLITE_PROFILE_MODE_MEMORY                  =(   4);

    /****************************************************************
    * フラグ
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_FLAG_NONE
    * @brief 通常モード
    * 
    * \~english
    * @def AILIA_TFLITE_FLAG_NONE
    * @brief Normal mode
    */
    public const UInt32  AILIA_TFLITE_FLAG_NONE                =(   0);

    /**
    * \~japanese
    * @def AILIA_TFLITE_FLAG_INPUT_OUTPUT_TENSORS_USE_SCRATCH
    * @brief 入出力テンソルをスクラッチバッファに確保する
    *
    * \~english
    * @def AILIA_TFLITE_FLAG_INPUT_OUTPUT_TENSORS_USE_SCRATCH
    * @brief Allocate input and output tensors in the scratch buffer.
    */
    public const UInt32  AILIA_TFLITE_FLAG_INPUT_AND_OUTPUT_TENSORS_USE_SCRATCH                =(   1);

    /****************************************************************
    * CPU拡張命令情報
    **/

    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_NONE
    * @brief 拡張命令無効
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_NONE
    * @brief CPU extension disabled
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_NONE                =(0x00000000);
    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_NEON
    * @brief NEON
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_NEON
    * @brief NEON
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_NEON                =(0x00000001);
    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_SSE2
    * @brief SSE2
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_SSE2
    * @brief SSE2
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_SSE2                =(0x00000002);
    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_SSE4_2
    * @brief SSE4.2
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_SSE4_2
    * @brief SSE4.2
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_SSE4_2              =(0x00000004);
    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_AVX
    * @brief AVX
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_AVX
    * @brief AVX
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_AVX                 =(0x00000008);
     /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_AVX2
    * @brief AVX2
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_AVX2
    * @brief AVX2
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_AVX2                =(0x00000010);
    /**
    * \~japanese
    * @def AILIA_TFLITE_CPU_FEATURES_VNNI
    * @brief VNNI
    * 
    * \~english
    * @def AILIA_TFLITE_CPU_FEATURES_VNNI
    * @brief VNNI
    */
    public const UInt32  AILIA_TFLITE_CPU_FEATURES_VNNI                =(0x00000020);

    /* Native Binary 定義 */

    #if (UNITY_IPHONE && !UNITY_EDITOR) || (UNITY_WEBGL && !UNITY_EDITOR)
        public const String LIBRARY_NAME="__Internal";
    #else
        #if (UNITY_ANDROID && !UNITY_EDITOR) || (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
            public const String LIBRARY_NAME="ailia_tflite";
        #else
            public const String LIBRARY_NAME="ailia_tflite";
        #endif
    #endif

    /****************************************************************
    * 推論環境列挙API
    **/

    /**
    * \~japanese
    * @brief 利用可能な計算環境の数を取得します
    * @param env_count 計算環境情報の数の格納先へのポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the number of available calculation environments
    * @param env_count Pointter to the number of calculation environment information
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetEnvironmentCount(ref UInt64 env_count);

    /**
    * \~japanese
    * @brief 計算環境の一覧を取得します
    * @param env 計算環境情報の格納先の配列
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   ailiaTFLiteGetEnvironmentCount()の返すサイズの配列を入力に渡してください。
    * 
    * \~english
    * @brief Get a list of calculation environments
    * @param env Arrangement of storage destination of calculation environmental information
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Give the size of the ailiaTFLiteGetEnvironmentCount() to input to the input.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetEnvironment(Int32 [] env);

    /****************************************************************
    * インスタンス生成・破棄API
    **/

    /**
    * \~japanese
    * @brief ailia TFLite runtimeのインスタンスを作成します。
    * @param instance      ailia TFLite runtimeインスタンスポインターへのポインター
    * @param tflite        tfliteモデルへのポインター
    * @param tflite_length tfliteの長さ(バイト単位)
    * @param pmalloc       mallocの関数ポインター(NULLの場合はmallocを利用)
    * @param pmemcpy       memcpyの関数ポインター(NULLの場合はmemcpyを利用)
    * @param pfree         freeの関数ポインター(NULLの場合はfreeを利用)
    * @param phandle       メモリアロケータに渡されるハンドル(標準のアロケータを使用する場合はNULL)
    * @param env_id        計算に利用する推論実行環境（AILIA_TFLITE_ENV_NNAPI_*）
    * @param memory_mode   メモリモード（AILIA_TFLITE_MEMORY_MODE_*）
    * @param flags         フラグ（AILIA_TFLITE_FLAG_*の論理和）
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   tfliteモデルを開き、ailia TFLite runtimeインスタンスを作成します。
    *   また、必要な内部バッファーの確保も行います。
    *   インスタンス生成に失敗した場合でinstanceにNULL以外が格納された場合はailiaTFLiteDestroyを呼び出す必要があります。
    * 
    * \~english
    * @brief Create ailia TFLite runtime instances.
    * @param instance      ailia TFLite runtime Pointter to instance pointer
    * @param tflite        Pointter to TFLITE model
    * @param tflite_length TFLITE length (byte unit)
    * @param pmalloc       MALLOC function pointer (use malloc in the case of null)
    * @param pmemcpy       MEMCPY function pointer (use Memcpy for null)
    * @param pfree         FREE function pointer (use free in the case of null)
    * @param phandle       Handle passed to Memoria Locator (NULL when using standard allocator)
    * @param env_id        Progress execution environment used for calculation (AILIA_TFLITE_ENV_NAPI_*)
    * @param memory_mode   Memory mode (AILIA_TFLITE_MEMORY_MODE_*)
    * @param flags         Flag (logical sum of AILIA_TFLITE_FLAG_*)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Open the TFLITE model and create ailia TFLite runtime instances.
    *   We will also secure the necessary internal buffers.
    *   If the instance fails, you need to call AILIATFLITEDESTROY if you store anything other than NULL in Instance.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteCreate(ref IntPtr instance, byte[] tflite, UInt64 tflite_length,
        IntPtr pmalloc, // must be IntPtr.Zero
        IntPtr pmemcpy, // must be IntPtr.Zero
        IntPtr pfree,   // must be IntPtr.Zero
        IntPtr phandle, // must be IntPtr.Zero
        Int32 env_id, Int32 memory_mode, UInt32 flags
    );

    /**
    * \~japanese
    * @brief ailia TFLite runtimeのインスタンスを破棄します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * 
    * \~english
    * @brief Discard the instance of ailia TFLite runtime.
    * @param instance      ailia TFLite runtime instance pointer
    */
    [DllImport(LIBRARY_NAME)]
    public static extern void ailiaTFLiteDestroy(IntPtr instance);

    /****************************************************************
    * CPU命令設定系API
    **/

    /**
    * \~japanese
    * @brief 使用するCPU命令を取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param cpu_features  AILIA_TFLITE_CPU_FEATURES_XXXの論理和
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   使用するCPU命令を取得します。
    *   デフォルトではCPU情報から取得したCPU命令を返します。
    *   ailiaTFLiteSetCpuFeaturesを呼び出した以降は、設定したCPU命令を返します。
    * 
    * \~english
    * @brief Get the CPU instruction to use
    * @param instance      ailia TFLite runtime instance pointer
    * @param cpu_features  AILIA_TFLITE_CPU_FEATURES_XXX logical sum
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the CPU instruction to use.
    *   By default, it returns the CPU instruction obtained from the CPU information.
    *   After calling ailiaTFLiteSetCpuFeatures, return the set CPU instruction.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetCpuFeatures(IntPtr instance, ref Int32 cpu_features);

    /**
    * \~japanese
    * @brief 使用するCPU命令を設定します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param cpu_features  AILIA_TFLITE_CPU_FEATURES_XXXの論理和
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   使用するCPU命令を設定します。
    *   使用できないCPU命令が設定された場合、AILIA_TFLITE_STATUS_OUT_OF_RANGEを返します。
    * 
    * \~english
    * @brief Set the CPU instruction to use
    * @param instance      ailia TFLite runtime instance pointer
    * @param cpu_features  AILIA_TFLITE_CPU_FEATURES_XXX logical sum
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Set the CPU instruction to use.
    *   If an unusable CPU instruction is set, return AILIA_TFLITE_STATUS_OUT_OF_RANGE.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteSetCpuFeatures(IntPtr instance, Int32 cpu_features);

    /****************************************************************
    * Device列挙/選択系API
    **/

    /**
    * \~japanese
    * @brief 使用可能なデバイスの個数を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param device_count  デバイス数の格納先
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   使用可能なデバイスの個数を取得します。
    *   ailiaTFLiteCreate()で指定するenv_idがNNAPI時のみ、動作可能です。
    * 
    * \~english
    * @brief Get the number of available devices.
    * @param instance      ailia TFLite runtime instance pointer
    * @param device_count  Device storage destination
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the number of available devices.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetDeviceCount(IntPtr instance, ref UInt64 device_count);

    /**
    * \~japanese
    * @brief 指定したインデックスのデバイスの名前を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param device_idx    ailiaTFLiteGetDeviceCount() で取得した数の中の、インデックス番号
    * @param name          デバイスの名前の文字列の格納先
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   指定したインデックスのデバイスの名前を取得します。
    *   ailiaTFLiteCreate()で指定するenv_idがNNAPI時のみ、動作可能です。
    * 
    * \~english
    * @brief Get the name of the specified index device.
    * @param instance      ailia TFLite runtime instance pointer
    * @param device_idx    Index number in the number acquired by AILIATFLITEGETDEVICECOUNT ()
    * @param name          Device storage destination with the name of the device
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the name of the specified index device.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetDeviceName(IntPtr instance, Int32 device_idx, ref IntPtr name);

    /**
    * \~japanese
    * @brief 指定したインデックスのデバイスの詳細情報を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param device_idx    ailiaTFLiteGetDeviceCount() で取得した数の中の、インデックス番号
    * @param info          デバイスの詳細情報の文字列の格納先
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   指定したインデックスのデバイスの詳細情報を取得します。
    * 
    * \~english
    * @brief Get the detailed information of the specified index device.
    * @param instance      ailia TFLite runtime instance pointer
    * @param device_idx    Index number in the number acquired by AILIATFLITEGETDEVICECOUNT ()
    * @param info          Detailed information of the device information string storage destination
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the detailed information of the specified index device.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetDeviceExtraInfo(IntPtr instance, Int32 device_idx, ref IntPtr info);

    /**
    * \~japanese
    * @brief 使用するデバイスのインデックスを指定します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param device_idxes  インデックス番号を格納した配列
    * @param idx_count     配列の数
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   使用するデバイスのインデックスを指定します。複数指定可能です。
    *   NNAPIにはDSP実装されていないオペレータが存在します。
    *   そのため、DSPとCPUの両方を指定することで非対応オペレータをCPUにオフロードする必要があります。
    * 
    * \~english
    * @brief Specify the index of the device to use.
    * @param instance      ailia TFLite runtime instance pointer
    * @param device_idxes  Arrangement with index number
    * @param idx_count     Number of arrays
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Specify the index of the device to use. Multiple specification can be specified.
    *   There are operators that are not implemented in NNAPI.
    *   Therefore, it is necessary to offload non compatible operators to CPU by specifying both DSP and CPU.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteSelectDevices(IntPtr instance, Int32 [] device_idxes, UInt64 idx_count);

    /**
    * \~japanese
    * @brief 使用するデバイスのインデックスおよび数を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param device_idxes  インデックスを格納する配列。ailiaTFLiteGetDeviceCount()で取得する数を最大数とする配列を用意してください。
    * @param idx_count     配列の数の格納先
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   使用するデバイスのインデックスおよび数を取得します。
    * 
    * \~english
    * @brief Get the index and number of devices used.
    * @param instance      ailia TFLite runtime instance pointer
    * @param device_idxes  A array that store indexes.Prepare an array with the maximum number of obtained in AILIATFLITEGETDEVICECOUNT ().
    * @param idx_count     Destination of the number of arrays
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the index and number of devices used.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetSelectedDeviceIndexes(IntPtr instance, Int32 [] device_idxes, ref UInt64 idx_count);

    /****************************************************************
    * 確保系API
    **/

    /**
    * \~japanese
    * @brief ailia TFLite runtimeの内部バッファーの確保を行います
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   内部の形状を更新し、推論に必要なバッファーを確保します。
    * 
    * \~english
    * @brief Ensure the internal buffer of ailia TFLite runtime
    * @param instance      ailia TFLite runtime instance pointer
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Update the internal shape and secure the buffer required for inference.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteAllocateTensors(IntPtr instance);

    /****************************************************************
    * Tensor設定系API
    **/

    /**
    * \~japanese
    * @brief 指定した入力indexのTensorの形状を変更します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param input_index   0~入力Tensorの数-1
    * @param shape         新しい形状
    * @param dim           次元
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   指定したTensorに未確定な次元が含まれていない場合AILIA_TFLITE_STATUS_FIXED_TENSORを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   入力Tensorの形状を変更します。
    *   この関数を呼び出すと確保済み内部バッファーを開放するため、
    *   ailiaTFLiteAllocateTensorsを呼び出す必要があります。
    *   また、入力形状に応じて中間Tensorの形状が変わる場合があるため、
    *   この関数を呼出後、ailiaTFLiteAllocateTensorsを呼び出すまでは取得系APIの結果が不正となる場合があります。
    *   indexの上限はailiaTFLiteGetNumberOfInputs()-1です。
    *   なお、指定したTensorに未確定な次元が含まれていない場合(ailiaTFLiteGetTensorShapeSignatureで取得したshapeに-1が含まれていない場合)はエラーとなります。
    * 
    * \~english
    * @brief Change the shape of Tensor of the specified input index
    * @param instance      ailia TFLite runtime instance pointer
    * @param input_index   0 ~ Input Tensor number-1
    * @param shape         New shape
    * @param dim           dimension
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If the specified Tensor does not contain an undecided dimension, return AILIA_TFLITE_STATUS_FIXED_TENSOR.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Change the shape of the input Tensor.
    *   Calling this function to open the secured internal buffer
    *   You need to call ailiaTFLiteAllocateTensors.
    *   Also, since the shape of the intermediate Tensor may change depending on the input shape.
    *   After calling this function, the results of the acquisition API may be fraudulent until the ailiaTFLiteAllocateTensors is called.
    *   The upper limit of INDEX is ailiaTFLiteGetNumberOfInputs() -1.
    *   If the specified Tensor does not contain an undecided dimension (if the Shape obtained in the ailiaTFLiteGetTensorShapeSignature does not include -1) will be an error.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteResizeInputTensor(IntPtr instance, Int32 input_index, Int32 [] shape, Int32 dim);

    /****************************************************************
    * Tensor取得系API
    **/

    /**
    * \~japanese
    * @brief tfliteモデルの入力Tensorの数を取得します。
    * @param instance             ailia TFLite runtimeインスタンスポインター
    * @param num_of_input_tensor  モデルの入力Tensorの数の格納先へのポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the number of TENSOR in the TFLITE model.
    * @param instance             ailia TFLite runtime instance pointer
    * @param num_of_input_tensor  Pointter to the number of model input TENSOR
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNumberOfInputs(IntPtr instance, ref Int32 num_of_input_tensor);

    /**
    * \~japanese
    * @brief 入力TensorのindexからTensorのindexを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_index  Tensorのindexの格納先へのポインター
    * @param input_index   入力Tensorのindex(0~入力Tensorの数-1)
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   input_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   入力TensorのindexからTensorのindexに変換します。
    *   indexの上限はailiaTFLiteGetNumberOfInputs()-1です。
    * 
    * \~english
    * @brief Get Tensor's index from index in Tensor.
    * @param instance      ailia TFLite runtime instance pointer
    * @param tensor_index  Pointer to the destination of Tensor's index
    * @param input_index   INDEX of input Tensor (0 ~ Number of input Tensor-1)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If INPUT_INDEX is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Convert from index in Tensor to index of Tensor.
    *   The upper limit of INDEX is ailiaTFLiteGetNumberOfInputs() -1.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetInputTensorIndex(IntPtr instance, ref Int32 tensor_index, Int32 input_index);

    /**
    * \~japanese
    * @brief tfliteモデルの出力Tensorの数を取得します。
    * @param instance             ailia TFLite runtimeインスタンスポインター
    * @param num_of_output_tensor モデルの出力Tensorの数の格納先へのポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the number of TENSOR of the TFLITE model.
    * @param instance             ailia TFLite runtime instance pointer
    * @param num_of_output_tensor Pointter to the number of model output TENSOR
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNumberOfOutputs(IntPtr instance, ref Int32 num_of_output_tensor);

    /**
    * \~japanese
    * @brief 出力TensorのindexからTensorのindexを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_index  Tensorのindexの格納先へのポインター
    * @param output_index  出力Tensorのindex(0~出力Tensorの数-1)
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   output_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   出力TensorのindexからTensorのindexに変換します。
    *   indexの上限はailiaTFLiteGetNumberOfOutputs()-1です。
    * 
    * \~english
    * @brief Obtain Tensor's index from index of output Tensor.
    * @param instance      ailia TFLite runtime instance pointer
    * @param tensor_index  Pointer to the destination of Tensor's index
    * @param output_index  Output TENSOR index (0 ~ Number of output Tensor-1)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If output_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Convert from Index in Tensor to Tensor Index.
    *   The upper limit of INDEX is ailiaTFLiteGetNumberOfOutputs () -1.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetOutputTensorIndex(IntPtr instance, ref Int32 tensor_index, Int32 output_index);

    /**
    * \~japanese
    * @brief index番目のTensorの次元を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_dim    指定したTensorの次元の格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the dimension of index number Tensor.
    * @param instance      ailia TFLite runtime instance pointer
    * @param tensor_dim    Pointter to the specified Tensor's dimension
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorDimension(IntPtr instance, ref Int32 tensor_dim, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの形状を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param shape         Tensorの形状の格納先
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   指定したTensorの形状をshapeに格納します。
    *   shapeはailiaTFLiteGetTensorDimensionで取得した次元以上のバッファーを確保してください。
    * 
    * \~english
    * @brief Get the shape of the index number Tensor.
    * @param instance      ailia TFLite runtime instance pointer
    * @param shape         Tensor shape storage destination
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Store the shape of the specified Tensor in Shape.
    *   Shape should secure a buffer that is more than the dimensions acquired by ailiaTFLiteGetTensorDimension.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorShape(IntPtr instance, Int32 [] shape, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの未確定な次元情報付き形状を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param shape         Tensorの形状の格納先
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   指定したTensorの未確定な次元情報付き形状をshapeに格納します。
    *   形状が未確定な次元は-1が格納されます。
    *   指定したTensorに未確定な次元が含まれていない場合はailiaTFLiteGetTensorShapeと同じ結果となります。
    *   shapeはailiaTFLiteGetTensorDimensionで取得した次元以上のバッファーを確保してください。
    * 
    * \~english
    * @brief Get the undecided dimensional form of Tensor of index number.
    * @param instance      ailia TFLite runtime instance pointer
    * @param shape         Tensor shape storage destination
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Store the undecided dimensional information of the specified Tensor in Shape.
    *   -1 is stored for dimensions with undecided shape.
    *   If the specified Tensor does not contain an undecided dimension, it will be the same result as ailiaTFLiteGetTensorShape.
    *   Shape should secure a buffer that is more than the dimensions acquired by ailiaTFLiteGetTensorDimension.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorShapeSignature(IntPtr instance, Int32 [] shape, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorのデータタイプを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_type   指定したTensorのデータタイプの格納先
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the Data type of TENSOR in Index.
    * @param instance      ailia TFLite runtime instance pointer
    * @param tensor_type   Specified TENSOR data type storage destination
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorType(IntPtr instance, ref sbyte tensor_type, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorのデータの格納バッファーを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param buffer        データの格納バッファーへのポインターの格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   bufferで取得できる格納バッファーの寿命はailiaTFLiteAllocateTensors/ailiaTFLiteResizeInputTensor/ailiaTFLiteDestroyのいずれか呼び出すまで有効です。
    *   また、格納バッファーの値はailiaTFLitePredictを呼び出すと変更されます。
    *   なお、呼び出し元で格納バッファーを開放する必要はありません。
    * 
    * \~english
    * @brief Obtain a storage buffer in Tensor's data.
    * @param instance      ailia TFLite runtime instance pointer
    * @param buffer        Pointter to the destination of the pointer to the data storage buffer
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   The life of the storage buffer that can be obtained with Buffer is valid until either ailiaTFLiteAllocateTensors/ailiaTFLiteResizeInputTensor/ailiaTFLiteDestroy is called.
    *   The value of the storage buffer is changed when ailiaTFLitePredict is called.
    *   There is no need to open the storage buffer at the caller.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorBuffer(IntPtr instance, ref IntPtr buffer, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの名前を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param name          Tensorの名前の文字列ポインターの格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   nameで取得できる文字列ポインターの寿命はailiaTFLiteDestroyを呼び出すまで有効です。
    *   なお、呼び出し元で文字列ポインターを開放する必要はありません。
    * 
    * \~english
    * @brief Get the name of the index number Tensor.
    * @param instance      ailia TFLite runtime instance pointer
    * @param name          Pointter to the destination of the character string pointer named Tensor
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   The life of the string pointer that can be obtained in name is effective until calling ailiaTFLiteDestroy.
    *   There is no need to open the string pointer at the caller.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorName(IntPtr instance, ref IntPtr name, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの量子化パラメーターの個数を取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param count         量子化パラメーターの個数の格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   指定したTensorに関連付けられた量子化パラメーターの個数を取得します。
    *   指定したTensorに量子化パラメーターが存在しない場合は0が格納されます。
    * 
    * \~english
    * @brief Get the number of quantified parameters of index number Tensor
    * @param instance      ailia TFLite runtime instance pointer
    * @param count         Pointter to the number of quantified parameters
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Get the number of quantified parameters associated with the specified Tensor.
    *   If there is no quantization parameter in the specified Tensor, 0 is stored.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorQuantizationCount(IntPtr instance, ref Int32 count, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの量子化パラメーターのスケールを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param scale         量子化パラメーターのスケールの格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   指定したTensorに量子化パラメーターが存在しない場合はAILIA_TFLITE_STATUS_PARAMETER_NOT_FOUNDが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   Tensorは以下の式で量子化して格納します。
    *   量子化値 = zero_point * round(入力/scale)
    *   scale:ailiaTFLiteGetTensorQuantizationScaleで取得
    *   zero_point:ailiaTFLiteGetTensorQuantizationZeroPointで取得
    *   scaleはailiaTFLiteGetTensorQuantizationCountで取得した要素数以上のバッファーを確保してください。
    * 
    * \~english
    * @brief Get the scale of the quantified parameter of the index number TENSOR.
    * @param instance      ailia TFLite runtime instance pointer
    * @param scale         Pointter to the storage destination of the quantized parameter
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   If the quantized parameter does not exist in the specified Tensor, AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND is returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Tensor is quantized and stored in the following formula.
    *   Quantization value = zero_point * round(input/scale)
    *   Acquired by scale: ailiaTFLiteGetTensorQuantizationScale
    *   Acquired by zero_point: ailiaTFliteGetTensorQuantizationZeroPoint
    *   scale should secure more than the number of elements obtained in ailiaTFLiteGetTensorQuantizationCount.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorQuantizationScale(IntPtr instance, float[] scale, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの量子化パラメーターのゼロ点を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param zero_point    量子化パラメーターのゼロ点の格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   指定したTensorに量子化パラメーターが存在しない場合はAILIA_TFLITE_STATUS_PARAMETER_NOT_FOUNDが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   量子化の解説はailiaTFLiteGetTensorQuantizationScaleを御覧ください。
    *   zero_pointはailiaTFLiteGetTensorQuantizationCountで取得した要素数以上のバッファーを確保してください。
    * 
    * \~english
    * @brief Get the zero point of the quantified parameter of the index number TENSOR.
    * @param instance      ailia TFLite runtime instance pointer
    * @param zero_point    Pointter to the destination of zero points of quantization parameters
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   If the quantized parameter does not exist in the specified Tensor, AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND is returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   See ailiaTFLiteGetTensorQuantizationScale for description of quantification.
    *   For zero_point, secure a buffer that is more than the number of elements acquired in ailiaTFLiteGetTensorQuantizationCountで.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorQuantizationZeroPoint(IntPtr instance, Int64[] zero_point, Int32 tensor_index);

    /**
    * \~japanese
    * @brief index番目のTensorの量子化パラメーターの軸を取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param axis          量子化パラメーターの軸の格納先へのポインター
    * @param tensor_index  Tensorのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   tensor_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   指定したTensorに量子化パラメーターが存在しない場合はAILIA_TFLITE_STATUS_PARAMETER_NOT_FOUNDが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the axis of the integrated TENSOR quantization parameter.
    * @param instance      ailia TFLite runtime instance pointer
    * @param axis          Pointter to the destination of the axis of the quantized parameter
    * @param tensor_index  Tensor's index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If Tensor_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   If the quantized parameter does not exist in the specified Tensor, AILIA_TFLITE_STATUS_PARAMETER_NOT_FOUND is returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetTensorQuantizationQuantizedDimension(IntPtr instance, ref Int32 axis, Int32 tensor_index);

    /****************************************************************
    * Tensor推論系API
    **/

    /**
    * \~japanese
    * @brief 推論を行います。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Do inference.
    * @param instance      ailia TFLite runtime instance pointer
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLitePredict(IntPtr instance);

    /****************************************************************
    * ノード情報取得API
    **/

    /**
    * \~japanese
    * @brief Nodeの個数を取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param count         Nodeの個数の格納先へのポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the number of node
    * @param instance      ailia TFLite runtime instance pointer
    * @param count         Pointter to the number of node stored
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeCount(IntPtr instance, ref Int32 count);

    /**
    * \~japanese
    * @brief node_index番目のNodeのOperatorを取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param op            Operatorの格納先へのポインター
    * @param node_index    Nodeのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   Operatorのenum値はtfliteファイルの内部の値に準じます。
    *   詳しくはTensorFlowのソースツリーのtensorflow/lite/schema/schema.fbsファイルを参照してください。
    * 
    * \~english
    * @brief Get node_index number Operator
    * @param instance      ailia TFLite runtime instance pointer
    * @param op            Pointter to the destination of Operator
    * @param node_index    Node index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If node_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   The ENUM value of Operator is the same as the internal value of the TFLite file.
    *   For more information, see TensorFlow/Lite/Schema/Schema.fbs files in TensorFlow source tree.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeOperator(IntPtr instance, ref Int32 op, Int32 node_index);

    /**
    * \~japanese
    * @brief node_index番目のNodeの入力の個数を取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param count         入力の個数の格納先へのポインター
    * @param node_index    Nodeのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Node_index gets the number of node input
    * @param instance      ailia TFLite runtime instance pointer
    * @param count         Pointter to the storage destination of the number of input
    * @param node_index    Node index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If node_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeInputCount(IntPtr instance, ref Int32 count, Int32 node_index);

    /**
    * \~japanese
    * @brief node_index番目のNodeのinput_index番目の入力のTensorのindexを取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_index  Tensorのindexの格納先へのポインター
    * @param node_index    Nodeのindex
    * @param input_index   入力のindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexおよびinput_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Node_index Acquires the index of Tensor in the input input of node.
    * @param instance      ailia TFLite runtime instance pointer
    * @param tensor_index  Pointer to the destination of Tensor's index
    * @param node_index    Node index
    * @param input_index   INDEX for input
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If node_index and input_index are out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeInputTensorIndex(IntPtr instance, ref Int32 tensor_index, Int32 node_index, Int32 input_index);

    /**
    * \~japanese
    * @brief node_index番目のNodeの出力の個数を取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param count         出力の個数の格納先へのポインター
    * @param node_index    Nodeのindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Node_index gets the number of node output
    * @param instance      ailia TFLite runtime instance pointer
    * @param count         Pointter to the storage destination of the number of output
    * @param node_index    Node index
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If node_index is out of range, AILIA_TFLITE_STATUS_OUT_OF_RANGE will be returned.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeOutputCount(IntPtr instance, ref Int32 count, Int32 node_index);

    /**
    * \~japanese
    * @brief node_index番目のNodeのoutput_index番目の出力のTensorのindexを取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param tensor_index  Tensorのindexの格納先へのポインター
    * @param node_index    Nodeのindex
    * @param output_index  出力のindex
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexおよびoutput_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeOutputTensorIndex(IntPtr instance, ref Int32 tensor_index, Int32 node_index, Int32 output_index);

    /**
    * \~japanese
    * @brief node_index番目のNodeのオプションを取得します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param value         出力の格納先へのポインター
    * @param node_index    Nodeのindex
    * @param key           オプション名の文字列ポインター
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   node_indexが範囲外の場合はAILIA_TFLITE_STATUS_OUT_OF_RANGEが返ります。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetNodeOption(IntPtr instance, IntPtr value, Int32 node_index, string key);

    /**
    * \~japanese
    * @brief Operatorの名前を取得します。
    * @param name          Operatorの名前の文字列ポインターの格納先へのポインター
    * @param op            Operator
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   nameで取得できる文字列ポインターの寿命はライブラリのアンロードまで有効です。
    *   なお、呼び出し元で文字列ポインターを開放する必要はありません。
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetOperatorName(ref IntPtr name, Int32 op);

    /****************************************************************
    * プロファイルAPI
    **/

    /**
    * \~japanese
    * @brief プロファイルモードをセットします。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param mode          プロファイルモード (AILIA_TFLITE_PROFILE_MODE_*)
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   プロファイルモードを指定します。デフォルトは無効です。
    *   プロファイルモードを有効にした場合、ailiaTFLiteGetSummaryの出力にプロファイル結果が追加されます。
    *   ailiaTFLiteCreateの直後に実行する必要があります。
    *   ailiaTFLiteAllocateTensorsの後に呼び出した場合はAILIA_TFLITE_STATUS_INVALID_STATEが返ります。
    * 
    * \~english
    * @brief Set the profile mode.
    * @param instance      ailia TFLite runtime instance pointer
    * @param mode          Profile mode (AILIA_TFLITE_PROFILE_MODE_*)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Specify the profile mode.The default is invalid.
    *   When the profile mode is enabled, the profile result is added to the output of ailiaTFLiteGetSummary.
    *   It must be executed immediately after ailiaTFLiteCreate.
    *   If you call after ailiaTFLiteAllocateTensors, AILIA_TFLITE_STATUS_INVALID_STATE will be returned.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteSetProfileMode(IntPtr instance, Int32 mode);

    /**
    * \~japanese
    * @brief ネットワークSummary用に必要なバッファのサイズを取得します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param buffer_size   バッファのサイズの格納先へのポインター(終端null文字分を含む)
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Get the size of the buffer required for network Summary.
    * @param instance      ailia TFLite runtime instance pointer
    * @param buffer_size   Pointter to the storage destination of the size of the buffer (including the terminal NULL character)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetSummaryLength(IntPtr instance, ref UInt64 buffer_size);

    /**
    * \~japanese
    * @brief 各Nodeの名前と形状を表示します。
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param buffer        Summaryの出力先の文字列ポインター
    * @param buffer_size   出力バッファのサイズ(終端null文字分を含む)。 ailiaTFLiteGetSummaryLength() で取得した値を設定してください。
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   その他のエラーの場合はAILIA_TFLITE_STATUS_XXXを返します。
    * 
    * \~english
    * @brief Displays the name and shape of each node.
    * @param instance      ailia TFLite runtime instance pointer
    * @param buffer        SUMMARY output string pointer
    * @param buffer_size   The size of the output buffer (including the terminal NULL character).Set the value obtained by AILIATFLITITEGETSUMMARYLENGTH ().
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   In the case of other errors, return AILIA_TFLITE_STATUS_XXX.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetSummary(IntPtr instance, byte[] buffer, UInt64 buffer_size);

    /****************************************************************
    * エラー詳細取得API
    **/

    /**
    * \~japanese
    * @brief エラーの詳細を返します
    * @param instance      ailia TFLite runtimeインスタンスポインター
    * @param buffer        エラー詳細文字列へのポインタ
    * @details
    *   文字列は解放する必要はありません。
    *   文字列の有効期間は次にailiaのAPIを呼ぶまでとなります。
    * 
    * \~english
    * @brief Returns the error details
    * @param instance      ailia TFLite runtime instance pointer
    * @param buffer        Error detailed pointer to string
    * @details
    *   The string does not need to be released.
    *   The validity period of the string is to call the AILIA API.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetErrorDetail(IntPtr instance, ref IntPtr buffer);

    /****************************************************************
    * スクラッチバッファ設定API
    **/

    /**
    * \~japanese
    * @brief スクラッチバッファを設定します
    * @param instance          ailia TFLite runtimeインスタンスポインター
    * @param int_buffer        スクラッチバッファ（L2）へのポインタ
    * @param int_buffer_size   スクラッチバッファ（L2）のサイズ（64byte以上）
    * @param mid_buffer        スクラッチバッファ（MSMC）へのポインタ
    * @param mid_buffer_size   スクラッチバッファ（MSMC）のサイズ
    * @param ext_buffer        スクラッチバッファ（DDR）へのポインタ
    * @param ext_buffer_size   スクラッチバッファ（DDR）のサイズ
    * @details
    *   MMALIBで使用するスクラッチバッファを設定します。ENV_MMALIBを指定した場合のみ有効です。
    *   バッファのアライメントは内部で行われるため、アライメントを考慮する必要はありません。
    *   L2以外のバッファサイズに0を指定すると、指定したバッファを無効化することが可能です。
    *   ENV_MMALIBを指定した場合に、この関数を呼び出さずにailiaTFLiteAllocateTensorsを実行すると、AILIA_TFLITE_STATUS_INVALID_STATEを返します。
    *   スクラッチバッファはailiaTFLitePredictを呼び出す度に変更することが可能です。
    *   ただし、スクラッチバッファのサイズはailiaTFLiteAllocateTensorsで使用した値と同じである必要があります。
    *   非永続化データのワーク領域として使用するため、シングルスレッドの場合は複数インスタンスに同じバッファを与えることが可能です。
    * 
    * \~english
    * @brief Set a scratch buffer
    * @param instance          ailia TFLite runtime instance pointer
    * @param int_buffer        Pointer to scratch buffer (L2)
    * @param int_buffer_size   Size of scratch buffer (L2) (64 byte or above)
    * @param mid_buffer        Pointer to scratch buffer (MSMC)
    * @param mid_buffer_size   Size of scratch buffer (MSMC)
    * @param ext_buffer        Pointer to scratch buffer (DDR)
    * @param ext_buffer_size   Size of scratch buffer (DDR)
    * @details
    *   Set the scratch buffer used for mmalib.It is valid only when env_mmalib is specified.
    *   The alignment of the buffer is performed internally, so there is no need to consider alignment.
    *   If you specify 0 for a buffer size other than L2, you can disable the specified buffer.
    *   If you specify * env_mmalib, execute ailiaTFLiteAllocateTensors without calling this function, return AILIA_TFLITE_STATUS_INVALID_STATE.
    *   The scratch buffer can be changed each time ailiaTFLitePredict is called.
    *   However, the size of the scratch buffer must be the same as the value used in ailiaTFLiteAllocateTensors.
    *   Because it is used as a work area of non -permanent data, it is possible to give the same buffer to multiple instances for single threads.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteSetScratchBuffer(IntPtr instance,
        IntPtr int_buffer, UInt64 int_buffer_size,
        IntPtr mid_buffer, UInt64 mid_buffer_size,
        IntPtr ext_buffer, UInt64 ext_buffer_size
    );

    /**
    * \~japanese
    * @brief スクラッチバッファの最大使用量を取得します。
    * @param instance          ailia TFLite runtimeインスタンスポインター
    * @param int_buffer_size   スクラッチバッファ（L2）のサイズ
    * @param mid_buffer_size   スクラッチバッファ（MSMC）のサイズ
    * @param ext_buffer_size   スクラッチバッファ（DDR）のサイズ
    * @details
    *   初回に大きいスクラッチバッファサイズで推論を行うことで、必要なスクラッチバッファのサイズを取得することができます。
    *   内部で行われるアライメントを考慮した値が返るため、取得した値はアライメントを考慮せずに直接使用可能です。
    * 
    * \~english
    * @brief Get the maximum usage of the scratch buffer.
    * @param instance          ailia TFLite runtime instance pointer
    * @param int_buffer_size   Size of scratch buffer (L2)
    * @param mid_buffer_size   Size of scratch buffer (MSMC)
    * @param ext_buffer_size   Size of scratch buffer (DDR)
    * @details
    *   You can get the size of the necessary scratch buffer by inferring in the first large scratch buffer size.
    *   Since the value considering the alignment performed inside is returned, the acquired value can be used directly without considering the alignment.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteGetScratchBufferUsage(IntPtr instance, ref UInt64 int_buffer_size, ref UInt64 mid_buffer_size, ref UInt64 ext_buffer_size);

    /****************************************************************
    * MKL設定API
    **/

    /**
    * \~japanese
    * @brief MKLが使用するスレッド数を設定します
    * @param num_threads   スレッド数（1以上）
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   mkl_set_num_threadsを呼び出すことでMKLのスレッド数を変更します。全てのインスタンスに影響します。
    *   デフォルトでは自動設定となります。
    *   MKLを使用しない環境（macOSなど）ではAILIA_TFLITE_STATUS_INVALID_STATEを返します。
    * 
    * \~english
    * @brief Set the number of threads used by MKL
    * @param num_threads   Number of threads (1 or more)
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   Change the MKL thread number by calling mkl_set_num_threads.It affects all instances.
    *   By default, it will be automatically set.
    *   In an environment that does not use Mkl (MacOS, etc.), return AILIA_TFLITE_STATUS_INVALID_STATE.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteMklSetNumThreads(Int32 num_threads);

    /**
    * \~japanese
    * @brief MKLのFastMMを無効化します
    * @return
    *   成功した場合はAILIA_TFLITE_STATUS_SUCCESSを返します。
    *   失敗した場合はAILIA_TFLITE_STATUS_XXXを返します。
    * @details
    *   mkl_disable_fast_mmを呼び出すことでFastMMを無効化します。全てのインスタンスに影響します。
    *   MKLはスレッドごとにメモリを確保するため、mkl_free_buffersを呼び出すまでメモリが単調増加します。
    *   このワークメモリは、ailiaTFLiteDestroyでmkl_free_buffersを呼び出すまで保持されます。
    *   FastMMを無効化することで、スレッドごとにメモリを確保しないように指定することができます。
    *   MKLを使用しない環境（macOSなど）ではAILIA_TFLITE_STATUS_INVALID_STATEを返します。
    * 
    * \~english
    * @brief Disable the mkl fastmm
    * @return
    *   If you succeed, return AILIA_TFLITE_STATUS_SUCCESS.
    *   If you fail, return AILIA_TFLITE_STATUS_XXX.
    * @details
    *   mkl_disable_fast_mm is called to disable FastMM.It affects all instances.
    *   Mkl increases the memory monotonously until mkl_Free_buffers is called to secure memory for each thread.
    *   This work memory is kept until mkl_Free_buffers is called in ailiaTFLiteDestroy.
    *   By disabling the fastmm, you can specify not to secure memory for each thread.
    *   In an environment that does not use Mkl (MacOS, etc.), return AILIA_TFLITE_STATUS_INVALID_STATE.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern Int32 ailiaTFLiteMklDisableFastMM();

    /**
    * \~japanese
    * @brief ライブラリバージョンを取得します。
    * @return
    *   バージョン番号(Marshal.PtrToStringAnsiでstringに変換可能)
    * @details
    *   返値は解放する必要はありません。
    *
    * \~english
    * @brief   Get the library version.
    * @return
    *   Version number (can be converted to string with Marshal.PtrToStringAnsi)
    * @details
    *   The return value does not need to be released.
    */
    [DllImport(LIBRARY_NAME)]
    public static extern IntPtr ailiaTFLiteGetVersion();
}
} // ailiaTFLite
