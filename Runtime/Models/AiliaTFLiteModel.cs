/**
* \~japanese
* @file
* @brief ailia TFLite Runtime Unity Plugin Model Class
* @author AXELL Corporation
* @date  February 6, 2023
* 
* \~english
* @file
* @brief ailia TFLite Runtime Unity Plugin Model Class
* @author AXELL Corporation
* @date  February 6, 2023
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ailiaTFLite{

public class AiliaTFLiteModel : IDisposable {
    protected IntPtr instance = IntPtr.Zero;

    private GCHandle model_mem_handle;
    private bool b_model_mem_handle = false;    // save if model_mem_handle is allocated.

    protected bool logging = true;
    private void log(object message){
        if(logging){
            Debug.Log(message);
        }
    }
    private bool CheckStatus(Int32 status, object funcname){
        if(status != AiliaTFLite.AILIA_TFLITE_STATUS_SUCCESS){
            log(funcname + " failed: " + status);
            return false;
        }
        return true;
    }

    private static byte[] ReadFile(string filePath){
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        fs.Close();
        return buffer;
    }

    /**
    * \~japanese
    * @brief ネットワークオブジェクトを破棄します。
    * @details
    *   ネットワークオブジェクトを破棄し、初期化します。
    *   
    *  \~english
    * @brief   Destroys network objects.
    * @details
    *   Destroys and initializes the network object.
    */
    public void Close(){
        CloseInstance();
        ReleaseMem();
    }

    /**
    * \~japanese
    * @brief リソースを解放します。
    *   
    *  \~english
    * @brief   Release resources.
    */
    public virtual void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing){
            // release managed resource
        }
        Close(); // release unmanaged resource
    }

    ~AiliaTFLiteModel(){
        Dispose(false);
    }

    private void CloseInstance(){
        if(instance != IntPtr.Zero){
            AiliaTFLite.ailiaTFLiteDestroy(instance);
            instance = IntPtr.Zero;
        }
    }

    /**
    * \~japanese
    * @brief メモリからネットワークオブジェクトを作成します。
    * @param tflite_model_buf   tfliteファイルのデータへのポインタ
    * @param env_id   計算に利用する推論実行環境（AILIA_TFLITE_ENV_NNAPI_*）
    * @param memory_mode   メモリモード（AILIA_TFLITE_MEMORY_MODE_*）
    * @param flags   フラグ（AILIA_TFLITE_FLAG_*の論理和）
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    * @detail
    *   メモリからネットワークオブジェクトを作成します。
    *   
    * \~english
    * @brief   Creates network objects from memory.
    * @param tflite_model_buf   Pointer to data in tflite file 
    * @param env_id   Progress execution environment used for calculation (AILIA_TFLITE_ENV_NAPI_*)
    * @param memory_mode   Memory mode (AILIA_TFLITE_MEMORY_MODE_*)
    * @param flags   Flag (logical sum of AILIA_TFLITE_FLAG_*)
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    * @detail
    *   Creates network objects from memory.
    */
    public bool OpenMem(byte [] tflite_model_buf, Int32 env_id, Int32 memory_mode, UInt32 flags){
        Int32 status;

        if(tflite_model_buf == null || tflite_model_buf.Length == 0){
            log("tflite_model_buf is empty.");
            return false;
        }
        CloseInstance();
        AiliaTFLiteLicense.CheckAndDownloadLicense();

        // fix the address
        model_mem_handle = GCHandle.Alloc(tflite_model_buf, GCHandleType.Pinned);
        b_model_mem_handle = true;

        status = AiliaTFLite.ailiaTFLiteCreate(ref instance, tflite_model_buf, (UInt32)tflite_model_buf.Length,
            IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 
            env_id, memory_mode, flags
        );
        if(!CheckStatus(status, "ailiaTFLiteCreate")) return false;

        return true;
    }

    /**
    * \~japanese
    * @brief モデルファイルからネットワークオブジェクトを作成します。
    * @param tflite_model_path   　tfliteファイルのパス名(MBSC or UTF16)
    * @param env_id   計算に利用する推論実行環境（AILIA_TFLITE_ENV_NNAPI_*）
    * @param memory_mode   メモリモード（AILIA_TFLITE_MEMORY_MODE_*）
    * @param flags   フラグ（AILIA_TFLITE_FLAG_*の論理和）
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    * @detail
    *   モデルファイルからネットワークオブジェクトを作成します。
    *   
    * \~english
    * @brief   Create a network object from a model file.
    * @param tflite_model_path  Pathname of the tflite file(MBSC or UTF16)
    * @param env_id   Progress execution environment used for calculation (AILIA_TFLITE_ENV_NAPI_*)
    * @param memory_mode   Memory mode (AILIA_TFLITE_MEMORY_MODE_*)
    * @param flags   Flag (logical sum of AILIA_TFLITE_FLAG_*)
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    * @detail
    *   Create a network object from a model file.
    */
    public bool OpenFile(string tflite_model_path, Int32 env_id, Int32 memory_mode, UInt32 flags){
        if(tflite_model_path == null){
            log("tflite_model_path is empty.");
            return false;
        }
        byte[] data = ReadFile(tflite_model_path);

        return OpenMem(data, env_id, memory_mode, flags);
    }

    private void ReleaseMem(){
        if(b_model_mem_handle){
            model_mem_handle.Free();
            b_model_mem_handle = false;
        }
    }

    /** 
    * \~japanese
    * @brief デバイスを選択します。
    * @param device_list 選択されたデバイス
    * @param reference_only NNAPIのリファレンス実装の使用する（検証用）
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief   Select device
    * @param device_list Selected device name
    * @param reference_only Only use nnapi reference implementation (for debug)
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool SelectDevice(ref string device_list, bool reference_only = false){
        device_list = "";
        string all_device_list = "";

        // Get Device Count
        UInt64 device_count = 0;
        int status = AiliaTFLite.ailiaTFLiteGetDeviceCount(instance, ref device_count);
        if(!CheckStatus(status, "ailiaTFLiteGetDeviceCount")) return false;

        // Get Device Name and Select require device
        Int32 [] device_idxes = new Int32[device_count];
        UInt64 active_device_cnt = 0;
        for (UInt64 device_idx = 0; device_idx < device_count; device_idx++){
            IntPtr name = IntPtr.Zero;
            status = AiliaTFLite.ailiaTFLiteGetDeviceName(instance, (int)device_idx, ref name);
            if(!CheckStatus(status, "ailiaTFLiteGetDeviceName")) return false;
            if (device_list != ""){
                device_list += " , ";
            }
            string device_name = Marshal.PtrToStringAnsi(name);
            if (reference_only == false || device_name.Contains("reference")){
                device_list += device_name;
                device_idxes[active_device_cnt] = (Int32)device_idx;
                active_device_cnt = active_device_cnt + 1;
            }
            all_device_list += device_name;
        }

        // Apply selected device
        if (active_device_cnt == 0){
            device_list = all_device_list;
        }else{
            status = AiliaTFLite.ailiaTFLiteSelectDevices(instance, device_idxes, active_device_cnt);
            if(!CheckStatus(status, "ailiaTFLiteSelectDevices")) return false;
        }
        return true;
    }

    /** 
    * \~japanese
    * @brief 内部バッファーの確保を行います
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief   Ensure the internal buffer
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool AllocateTensors(){
        int status = AiliaTFLite.ailiaTFLiteAllocateTensors(instance);
        if(!CheckStatus(status, "ailiaTFLiteAllocateTensors")) return false;
        return true;
    }

    /** 
    * \~japanese
    * @brief 推論を行います。
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief   Perform inference.
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool Predict(){
        Int32 status;

        status = AiliaTFLite.ailiaTFLitePredict(instance);
        if(!CheckStatus(status, "ailiaTFLitePredict")) return false;

        return true;
    }

    /** 
    * \~japanese
    * @brief 入力テンソルを取得します。
    * @param shape 入力shape
    * @param buffer　入力バッファへのポインタ
    * @param tensor_type 入力バッファの型
    * @param quant_scale 量子化スケール
    * @param quant_zero_point 0ポイント
    * @param quant_axis 量子化の次元
    * @param input_index 入力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Get the input tensor.
    * @param shape Input Shape
    * @param buffer　Pointer to input buffer
    * @param tensor_type Type of input buffer
    * @param quant_scale Quantization scale
    * @param quant_zero_point 0 points
    * @param quant_axis Quantization axis
    * @param input_index Input tensor index
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool GetInputTensorInfo(ref Int32[] shape, ref IntPtr buffer, ref sbyte tensor_type,
        ref float [] quant_scale, ref Int64 [] quant_zero_point, ref Int32 quant_axis,
        Int32 input_index = 0
    ){
        Int32 status;
        Int32 num_of_input_tensor=-1;
        Int32 tensor_index=-1;
        Int32 tensor_dim=-1;

        status = AiliaTFLite.ailiaTFLiteGetNumberOfInputs(instance, ref num_of_input_tensor);
        if(!CheckStatus(status, "ailiaTFLiteGetNumberOfInputs")) return false;
        if(input_index >= num_of_input_tensor) return false;
        status = AiliaTFLite.ailiaTFLiteGetInputTensorIndex(instance, ref tensor_index, input_index);
        if(!CheckStatus(status, "ailiaTFLiteGetInputTensorIndex")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorDimension(instance, ref tensor_dim, tensor_index);
        shape = new Int32[tensor_dim];
        if(!CheckStatus(status, "ailiaTFLiteGetTensorDimension")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorShape(instance, shape, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorShape")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorBuffer(instance, ref buffer, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorBuffer")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorType(instance, ref tensor_type, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorType")) return false;

        // quantization params
        if (tensor_type != AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_FLOAT32){
            Int32 quant_count = 0;
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationCount(instance, ref quant_count, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationCount")) return false;
            quant_scale = new float[quant_count];
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationScale(instance, quant_scale, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationScale")) return false;
            quant_zero_point = new Int64[quant_count];
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationZeroPoint(instance, quant_zero_point, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationZeroPoint")) return false;
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationQuantizedDimension(instance, ref quant_axis, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationQuantizedDimension")) return false;
        }

        //log("input num_of_input_tensor:"+num_of_input_tensor+", index:"+tensor_index+", dim:"+tensor_dim+", shape:"+shape[0]+","+shape[1]+","+shape[2]+","+shape[3]+", quant_zero_point:"+quant_zero_point[0]+", quant_scale:"+quant_scale[0]+", tensor_type:"+tensor_type);
        return true;
    }

    /** 
    * \~japanese
    * @brief 出力テンソルを取得します。
    * @param shape 出力shape
    * @param buffer　出力バッファへのポインタ
    * @param tensor_type 出力バッファの型
    * @param quant_scale 量子化スケール
    * @param quant_zero_point 0ポイント
    * @param quant_axis 量子化の次元
    * @param output_index 出力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Get the output tensor.
    * @param shape Output Shape
    * @param buffer　Pointer to output buffer
    * @param tensor_type Type of output buffer
    * @param quant_scale Quantization scale
    * @param quant_zero_point 0 points
    * @param quant_axis Quantization axis
    * @param output_index Index of output tensor
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool GetOutputTensorInfo(ref Int32[] shape, ref IntPtr buffer, ref sbyte tensor_type,
        ref float [] quant_scale, ref Int64 [] quant_zero_point, ref Int32 quant_axis,
        Int32 output_index = 0
    ){
        Int32 status;
        Int32 num_of_output_tensor=-1;
        Int32 tensor_index=-1;
        Int32 tensor_dim=-1;

        status = AiliaTFLite.ailiaTFLiteGetNumberOfOutputs(instance, ref num_of_output_tensor);
        if(!CheckStatus(status, "ailiaTFLiteGetNumberOfOutputs")) return false;
        if(output_index >= num_of_output_tensor) return false;

        status = AiliaTFLite.ailiaTFLiteGetOutputTensorIndex(instance, ref tensor_index, output_index);
        if(!CheckStatus(status, "ailiaTFLiteGetOutputTensorIndex")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorDimension(instance, ref tensor_dim, tensor_index);
        shape = new Int32[tensor_dim];
        if(!CheckStatus(status, "ailiaTFLiteGetTensorDimension")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorShape(instance, shape, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorShape")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorBuffer(instance, ref buffer, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorBuffer")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorType(instance, ref tensor_type, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorType")) return false;

        // quantization params
        if (tensor_type != AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_FLOAT32){
            Int32 quant_count = 0;
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationCount(instance, ref quant_count, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationCount")) return false;
            quant_scale = new float[quant_count];
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationScale(instance, quant_scale, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationScale")) return false;
            quant_zero_point = new Int64[quant_count];
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationZeroPoint(instance, quant_zero_point, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationZeroPoint")) return false;
            status = AiliaTFLite.ailiaTFLiteGetTensorQuantizationQuantizedDimension(instance, ref quant_axis, tensor_index);
            if(!CheckStatus(status, "ailiaTFLiteGetTensorQuantizationQuantizedDimension")) return false;
        }

        //log("output num_of_output_tensor:"+num_of_output_tensor+", index:"+tensor_index+", dim:"+tensor_dim+", shape:"+shape[0]+","+shape[1]+","+shape[2]+", quant_count:"+quant_count+", quant_scale:"+quant_scale[0]+", quant_zero_point:"+quant_zero_point[0]+", quant_axis:"+quant_axis+", tensor_type:"+tensor_type);
        return true;
    }

    /** 
    * \~japanese
    * @brief 入力テンソルの形状を取得します。
    * @param shape 入力shape
    * @param input_index 入力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Get the shape of input tensor.
    * @param shape Input Shape
    * @param input_index Input tensor index
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool GetInputTensorShape(ref int[] shape, int input_index)
    {
        Int32 status;
        Int32 num_of_input_tensor=-1;
        Int32 tensor_index=-1;
        Int32 tensor_dim=-1;

        status = AiliaTFLite.ailiaTFLiteGetNumberOfInputs(instance, ref num_of_input_tensor);
        if(!CheckStatus(status, "ailiaTFLiteGetNumberOfInputs")) return false;
        if(input_index >= num_of_input_tensor) return false;
        status = AiliaTFLite.ailiaTFLiteGetInputTensorIndex(instance, ref tensor_index, input_index);
        if(!CheckStatus(status, "ailiaTFLiteGetInputTensorIndex")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorDimension(instance, ref tensor_dim, tensor_index);
        shape = new Int32[tensor_dim];
        if(!CheckStatus(status, "ailiaTFLiteGetTensorDimension")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorShape(instance, shape, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorShape")) return false;
    
        return true;
    }

    /** 
    * \~japanese
    * @brief 出力テンソルの形状を取得します。
    * @param shape 出力shape
    * @param output_index 出力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Get the shape of output tensor.
    * @param shape Output Shape
    * @param output_index Index of output tensor
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool GetOutputTensorShape(ref int[] shape, int output_index)
    {
        Int32 status;
        Int32 num_of_output_tensor=-1;
        Int32 tensor_index=-1;
        Int32 tensor_dim=-1;

        status = AiliaTFLite.ailiaTFLiteGetNumberOfOutputs(instance, ref num_of_output_tensor);
        if(!CheckStatus(status, "ailiaTFLiteGetNumberOfOutputs")) return false;
        if(output_index >= num_of_output_tensor) return false;

        status = AiliaTFLite.ailiaTFLiteGetOutputTensorIndex(instance, ref tensor_index, output_index);
        if(!CheckStatus(status, "ailiaTFLiteGetOutputTensorIndex")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorDimension(instance, ref tensor_dim, tensor_index);
        shape = new Int32[tensor_dim];
        if(!CheckStatus(status, "ailiaTFLiteGetTensorDimension")) return false;
        status = AiliaTFLite.ailiaTFLiteGetTensorShape(instance, shape, tensor_index);
        if(!CheckStatus(status, "ailiaTFLiteGetTensorShape")) return false;

        return true;
    }

    /** 
    * \~japanese
    * @brief 入力テンソルのデータを設定します。
    * @param input_data 入力データ
    * @param input_index 入力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Set the data of input tensor.
    * @param input_data Input Data
    * @param input_index Input tensor index
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool SetInputTensorData(float [] input_data, int input_index){
        Int32 [] input_shape = null;
        IntPtr input_buffer = IntPtr.Zero;
        sbyte input_tensor_type = 0;
        float [] input_quant_scale = null;
        Int64 [] input_quant_zero_point = null;
        Int32 input_quant_axis = 0;
        GetInputTensorInfo(ref input_shape, ref input_buffer, ref input_tensor_type,
            ref input_quant_scale, ref input_quant_zero_point, ref input_quant_axis, input_index
        );
        int dst_data_size = input_data.Length;

        if (input_tensor_type == AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_FLOAT32){
            Marshal.Copy(input_data, 0, input_buffer, dst_data_size);
        }else{
            byte[] dst_data_ptr = new byte[dst_data_size];
            quant(dst_data_ptr, input_data, input_quant_scale[0], input_quant_zero_point[0], input_tensor_type);
            Marshal.Copy(dst_data_ptr, 0, input_buffer, dst_data_size);
        }

        return true;
    }

    /** 
    * \~japanese
    * @brief 出力テンソルのデータを取得します。
    * @param output_data 出力データ
    * @param output_index 出力テンソルのインデックス
    * @return
    *   成功した場合はtrue、失敗した場合はfalseを返す。
    *   
    * \~english
    * @brief Get the data of output tensor.
    * @param output_data Output Data
    * @param output_index Index of output tensor
    * @return
    *   If this function is successful, it returns  true  , or  false  otherwise.
    */
    public bool GetOutputTensorData(float [] output_data, int output_index){
        Int32 [] output_shape = null;
        IntPtr output_buffer = IntPtr.Zero;
        sbyte output_tensor_type = 0;
        float [] output_quant_scale = null;
        Int64 [] output_quant_zero_point = null;
        Int32 output_quant_axis = 0;

        GetOutputTensorInfo(ref output_shape, ref output_buffer, ref output_tensor_type,
            ref output_quant_scale, ref output_quant_zero_point, ref output_quant_axis, output_index
        );

        int output_size = output_data.Length;
        if (output_tensor_type == AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_FLOAT32){
            Marshal.Copy(output_buffer, output_data, 0, output_size);
        }else{
            byte[] buf = new byte[output_size];
            Marshal.Copy(output_buffer, buf, 0, output_size);
            dequant(output_data, buf, output_quant_scale[0], output_quant_zero_point[0], output_tensor_type);
        }
        return true;
    }

    /* 量子化ユーティリティ */
    private void quant(byte [] dst, float [] val, float quant_scale, Int64 quant_zero_point, int tensor_type){
        if (tensor_type == AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_UINT8){
            quant_uint8(dst, val, quant_scale, quant_zero_point);
        }else{
            quant_int8(dst, val, quant_scale, quant_zero_point);
        }
    }

    private void dequant(float [] dst, byte [] src, float quant_scale, Int64 quant_zero_point, int tensor_type){
        if (tensor_type == AiliaTFLite.AILIA_TFLITE_TENSOR_TYPE_UINT8){
            dequant_uint8(dst, src, quant_scale, quant_zero_point);
        }else{
            dequant_int8(dst, src, quant_scale, quant_zero_point);
        }
    }

    private void quant_uint8(byte [] dst, float [] src, float quant_scale, Int64 quant_zero_point){
        for (int i = 0; i < src.Length; i++){
            float val = (src[i] / quant_scale) + quant_zero_point;
            if (val > 255) val = 255;
            if (val < 0) val = 0;
            dst [i] = (byte)val;
        }
    }

    private void dequant_uint8(float [] dst, byte [] src, float quant_scale, Int64 quant_zero_point){
        for (int i = 0; i < src.Length; i++){
            dst [i] = ((float)src[i] - (float)quant_zero_point) * quant_scale;
        }
    }

    private void quant_int8(byte [] dst, float [] src, float quant_scale, Int64 quant_zero_point){
        for (int i = 0; i < src.Length; i++){
            float val = src[i];
            val = (val / quant_scale) + quant_zero_point;
            if (val > 127) val = 127;
            if (val < -128) val = -128;
            int val_int = (int)val;
            if (val_int < 0){
                val_int = val_int + 256;
            }
            dst [i] = (byte)val_int;
        }
    }

    private void dequant_int8(float [] dst, byte [] src, float quant_scale, Int64 quant_zero_point){
        for (int i = 0; i < src.Length; i++){
            int val_s = src[i];
            if (val_s >= 128){
                val_s = val_s - 256;
            }
            dst [i] = ((float)val_s - (float)quant_zero_point) * quant_scale;
        }
    }

    //プロファイルモード有効
    /**
    *  \~japanese
    *  @brief プロファイルモードを有効にします。
    *  @return
    *    成功した場合は true 、失敗した場合は false を返します。
    *  @detail
    *    プロファイルモードを有効にします。プロファイルモードを有効にして推論後、Summary APIでプロファイル結果を取得します。
    *
    *  \~english
    *  @brief   Enable profile mode
    *  @return
    *    Returns true on success, false on failure.
    *  @detail
    *    Enable profile mode. After enabling profile mode and inference, get profile results in Summary API.
    */
    public bool SetProfileMode(int profile_mode)
    {
        if (instance == null){
            return false;
        }
        int status = AiliaTFLite.ailiaTFLiteSetProfileMode(instance, profile_mode);
        if(!CheckStatus(status, "ailiaTFLiteSetProfileMode")) return false;
        return true;
    }

    /**
    * \~japanese
    * @brief ネットワーク情報およびプロファイル結果を取得します。
    * @return
    *   成功した場合はネットワーク情報およびプロファイル結果を示すASCII文字列を、失敗した場合は null を返します。
    * @detail
    *   ネットワーク情報およびプロファイル結果を含む文字列を取得します。
    *
    * \~english
    * @brief   Obtain network information and profile results
    * @return
    *   Returns an ASCII string displaying the network information and profile results on success, or null on failure.
    * @detail
    *   Obtains a string containing network information and profile results.
    */
    public string GetSummary()
    {
        if (instance == null){
            return "";
        }
        UInt64 buffer_size = 0;
        int status = AiliaTFLite.ailiaTFLiteGetSummaryLength(instance, ref buffer_size);
        if(!CheckStatus(status, "ailiaTFLiteGetSummaryLength")) return null;
        byte[] buffer = new byte[buffer_size];
        status = AiliaTFLite.ailiaTFLiteGetSummary(instance, buffer, buffer_size);
        if(!CheckStatus(status, "ailiaTFLiteGetSummary")) return null;
        return System.Text.Encoding.ASCII.GetString(buffer);
    }
}

} // ailiaTFLite
