import CryptoJS from 'crypto-js';

//加密函数
export function encrypt(data) {
    const key = CryptoJS.enc.Utf8.parse("ACXJDAH@#");  
    const iv = CryptoJS.enc.Utf8.parse("ACXJDAH2024@#");
    const content = CryptoJS.enc.Utf8.parse(data);
    const encrypted = CryptoJS.AES.encrypt(content, key, {
      iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    });
    return encrypted.toString();
  }