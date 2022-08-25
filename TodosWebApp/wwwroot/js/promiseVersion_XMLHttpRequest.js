/*利用Promise, 封装XMLHttpRequest对象
 * 1. param method=> Request Method: "GET", "POST"... -> String (ref: https://javascript.ruanyifeng.com/bom/ajax.html#toc15)
 * 2. param url=> Target URL
 * 3. param dataType=> Response Data Type: "json", "blob"...  -> String (ref: https://javascript.ruanyifeng.com/bom/ajax.html#toc5)
 * 4. param dataSended=> DataSended: null... -> ArrayBufferView, Blob, String, FormData, Document (ref: https://javascript.ruanyifeng.com/bom/ajax.html#toc16)
 **/
function promiseVersion_XMLHttpRequest(method, url, dataType, dataSended) {
    let p = new Promise((resovle, reject) => {
        let oXhr = new XMLHttpRequest();
        oXhr.open(method, url, true);
        oXhr.responseType = dataType;/* dataType */
        /* 服务器传来的数据接收完毕或者本次接收已经失败 */
        oXhr.onload = function (oEvent) {
            if (oXhr.status == 200) {/* success */
                resovle(oXhr);
            } else {/* error */
                reject(oXhr);
            }
        };
        oXhr.send(dataSended);
    });
    return p;
};

/**
 * 知识点:
 * 1. FormData API. 例如append(...)
 * 2. Blob API.
 * 3. Promise 理解. ref: https://zh.javascript.info/async, *Professional JavaScript for Web Developers, 4th Edition*, 
 * 4. XMLHttpRequest 理解. ref: https://javascript.ruanyifeng.com/bom/ajax.html
 * 5. Event 理解
 * 6. Function 理解
 * 7. FileReader API.
 * 8. HTTP 理解.
/** example 1 */
/* 下载文件
function downloadCheckResultXLSFile() {
    const url = "/OCRParser/OCRCheck/DownloadCheckResultXLSFile";
    const formData = new FormData();// (**important**)
    const checkBatchNum = document.getElementsByClassName("checkBatchNum")[0].value;
    formData.append("checkBatchNum", checkBatchNum);

    promiseVersion_XMLHttpRequest("POST", url, "blob", formData)
        .then((xhr) => {
            const blob = xhr.response;// (**important**)
            const reader = new FileReader();
            reader.readAsDataURL(blob); // 转换为base64，可以直接放入a表情href
            reader.onload = function (e) {
                let a = document.createElement('a');
                a.download = `${checkBatchNum}_比对结果.xls`;// 文件名
                a.href = e.target.result;
                document.documentElement.appendChild(a);
                a.click();
                a.remove(); // 等价于document.documentElement.removeChild(a);
            };
        }).catch((xhr) => {
            alert("Error " + xhr.status + " occurred when trying to download your file.");
        });
}
*/

/** example 2*/
/** 上传文件
function uploadExcelOfOutOfWarehouseInfo() {
    displayLoading(document.body);
    let form = document.forms.namedItem("form-excelUpload");
    let fileInput = document.querySelector("form[name='form-excelUpload'] > input[type='file']");
    let oData = new FormData(form);
    let url = "/OCRParser/OCRCheck/UploadExcelOfOutOfWarehouseInfo";

    //beforeSend
    if (!validate(fileInput))
        return;

    promiseVersion_XMLHttpRequest("POST", url, "json", oData)
        .then((xhr) => {
            handle(xhr.response);
        }).catch((xhr) => {
            alert("Error " + xhr.status + " occurred when trying to upload your file.");
        }).finally(() => {
            fileInput.value = '';
            displayNoneLoading(document.body);
        });

    function handle(res) {
        debugger;
        if (res.result.code == "0") {
            swal("上传成功!");
            console.log(res.result.data);
            renderExcelInfoList(res.result.data)
        } else {
            swal({
                title: "提示",
                text: res.msg,
                icon: "warning",
                button: "确认",
            });
        }
    }

    function validate(fileHTMLInputEle) {
        if (!canFilesUpload(fileHTMLInputEle)) {
            swal({
                title: "提示",
                text: "请上传①单个②Excel文件",
                icon: "info",
                button: "确认",
            });
            displayNoneLoading(document.body);
            fileHTMLInputEle.value = '';
            return false;
        }
        return true;

        function canFilesUpload(fileHTMLInputEle) {
            let result = true;
            oFiles = fileHTMLInputEle.files;
            nFiles = oFiles.length;
            // 仅限上传一个Excel文件
            if (nFiles > 1)
                result = false;
            if (!isFileTypeExcel(oFiles[0].type))
                result = false;
            return result;
        }

        function isFileTypeExcel(fileType) {
            let pattern = /vnd\.openxmlformats\-officedocument\.spreadsheetml\.sheet|vnd\.ms\-excel/i;//判断文件类型是不是Excel格式
            return pattern.test(fileType);
        }
    }
}
*/