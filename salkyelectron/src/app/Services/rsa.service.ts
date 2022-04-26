import { Injectable } from '@angular/core';
import { catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RsaService {
  constructor() {}

  public privKey?: CryptoKey;
  public publKey?: CryptoKey;

  private Name = 'RSA-OAEP';
  private ModulosLenght = 4096;
  private Exponent = new Uint8Array([1, 0, 1]);
  private Hash = 'SHA-512';
  ///

  static async GenerateNewKeys(): Promise<RsaService> {
    var rsaService = new RsaService();
    var keys = await window.crypto.subtle.generateKey(
      {
        name: 'RSA-OAEP',
        modulusLength: 4096,
        publicExponent: new Uint8Array([1, 0, 1]),
        hash: { name: 'SHA-512' },
      },
      true,
      ['encrypt', 'decrypt']
    );
    rsaService.publKey = keys.publicKey;
    rsaService.privKey = keys.privateKey;
    return rsaService;
  }

  static async FromPrivateKeyStr(privateKey: string): Promise<RsaService> {
    var rsaService = new RsaService();
    await rsaService.importFromBase64string(privateKey, true);
    var pubKeyStr = await rsaService.deriveRsaPublicKeyFromPrivateKeyPem(
      privateKey
    );
    await rsaService.importFromBase64string(pubKeyStr, false);
    return rsaService;
  }

  static async FromPublicKeyStr(pubKey: string): Promise<RsaService> {
    var rsaService = new RsaService();
    await rsaService.importFromBase64string(pubKey, false);
    return rsaService;
  }

  private async importFromBase64string(
    keystr: string,
    isPrivateKey: boolean
  ): Promise<void> {
    keystr = atob(keystr);
    var keyBuffer = new Uint8Array(keystr.length);
    for (let i = 0; i < keystr.length; i++) {
      keyBuffer[i] = keystr[i].charCodeAt(0);
    }
    var result = await window.crypto.subtle.importKey(
      isPrivateKey ? 'pkcs8' : 'spki',
      keyBuffer,
      {
        name: this.Name,
        hash: this.Hash,
      },
      true,
      isPrivateKey ? ['decrypt'] : ['encrypt']
    );
    if (isPrivateKey) this.privKey = result;
    else this.publKey = result;
  }

  public async Encrypt(dataStr: string): Promise<string> {
    if (!this.publKey) {
      throw new Error('Public Key cannot be null');
    }
    var buffer = this.stringToArrayBuffer(dataStr);
    var res = await window.crypto.subtle.encrypt(
      {
        name: this.Name,
        //label: Uint8Array([...]) //optional
      },
      this.publKey, //from generateKey or importKey above
      buffer //ArrayBuffer of data you want to encrypt
    );
    return btoa(this.arrayBufferToString(res));
  }
  public async Decrypt(encryptedStr: string): Promise<string> {
    if (!this.privKey) {
      throw new Error('Private Key cannot be null');
    }
    encryptedStr = atob(encryptedStr);
    var encryptBuff = this.stringToArrayBuffer(encryptedStr);
    var res = await window.crypto.subtle.decrypt(
      {
        name: this.Name,
        //label: Uint8Array([...]) //optional
      },
      this.privKey, //from generateKey or importKey above
      encryptBuff //ArrayBuffer of data you want to encrypt
    );
    return this.arrayBufferToString(res);
  }

  public async ExportPubKey(): Promise<string> {
    if (!this.publKey) {
      throw new Error('Public Key cannot be null');
    }
    return this.ExportKey(this.publKey, false);
  }

  public async ExportPrivateKey(): Promise<string> {
    if (!this.privKey) {
      throw new Error('Private Key cannot be null');
    }
    return this.ExportKey(this.privKey, true);
  }

  private async ExportKey(
    key: CryptoKey,
    isPrivateKey: boolean
  ): Promise<string> {
    if (!key) {
      throw new Error('Key cannot be null');
    }
    var keyBuffer: ArrayBuffer;
    if (isPrivateKey) keyBuffer = await crypto.subtle.exportKey('pkcs8', key);
    else keyBuffer = await crypto.subtle.exportKey('spki', key);
    var keyArr = new Uint8Array(keyBuffer);
    const KeyStr = String.fromCharCode.apply(null, keyArr as any);
    return btoa(KeyStr);
  }

  private stringToArrayBuffer(str: string): ArrayBuffer {
    var buf = new ArrayBuffer(str.length);
    var bufView = new Uint8Array(buf);
    for (var i = 0, strLen = str.length; i < strLen; i++) {
      bufView[i] = str.charCodeAt(i);
    }
    return buf;
  }
  // private arrayBufferToString(buffer: ArrayBuffer): string {
  //   var byteArray = new Uint8Array(buffer);
  //   var byteString = '';
  //   for (var i = 0; i < byteArray.byteLength; i++) {
  //     byteString += String.fromCodePoint(byteArray[i]);
  //   }
  //   return byteString;
  // }

  // ------------------- Derive Public Key -------------------  Derive Public Key--------------------  Derive Public Key ------------------- //
  // ------------------- Derive Public Key -------------------  Derive Public Key--------------------  Derive Public Key ------------------- //
  // ------------------- Derive Public Key -------------------  Derive Public Key--------------------  Derive Public Key ------------------- //
  // ------------------- Derive Public Key -------------------  Derive Public Key--------------------  Derive Public Key ------------------- //

  private async deriveRsaPublicKeyFromPrivateKeyPem(
    privateKeyPem: string
  ): Promise<string> {
    var pem = privateKeyPem;
    // fetch the part of the PEM string between header and footer
    // const pemHeader = "-----BEGIN PRIVATE KEY-----";
    // const pemFooter = "-----END PRIVATE KEY-----";
    // const pemContents = pem.substring(pemHeader.length, pem.length - pemFooter.length);
    // base64 decode the string to get the binary data
    const binaryDerString = window.atob(privateKeyPem);
    // convert from a binary string to an ArrayBuffer
    const binaryDer = this.str2ab(binaryDerString);
    // import rsa private key
    var privateKey = await window.crypto.subtle.importKey(
      'pkcs8', // can be "jwk" (public or private), "spki" (public only), or "pkcs8" (private only)
      binaryDer,
      {
        // these are the algorithm options
        name: 'RSASSA-PKCS1-v1_5',
        hash: { name: 'SHA-256' }, //can be "SHA-1", "SHA-256", "SHA-384", or "SHA-512"
      },
      true, //whether the key is extractable (i.e. can be used in exportKey)
      ['sign'] //"verify" for public key import, "sign" for private key imports
    );
    var keyData = await this.exportPrivateKeyJwk(privateKey); // the key is taken from privateKeyInternal
    var publicKey = await this.importPublicKeyJwk(keyData);
    var pubKeyStr = await this.exportPublicKey(publicKey);
    return pubKeyStr;
  }

  // step 2 export the rsa private key in jwk encoding
  private async exportPrivateKeyJwk(
    privateKey: CryptoKey
  ): Promise<JsonWebKey> {
    var keyData = await window.crypto.subtle.exportKey(
      'jwk', //can be "jwk" (public or private), "spki" (public only), or "pkcs8" (private only)
      privateKey //can be a publicKey or privateKey, as long as extractable was true
    );
    delete keyData.d;
    delete keyData.dp;
    delete keyData.dq;
    delete keyData.q;
    delete keyData.qi;
    delete keyData.p;
    keyData.key_ops = ['encrypt', 'verify'];
    return keyData;
  }

  private async importPublicKeyJwk(binaryJwk: JsonWebKey): Promise<CryptoKey> {
    var publicKey = await window.crypto.subtle.importKey(
      'jwk', // can be "jwk" (public or private), "spki" (public only), or "pkcs8" (private only)
      binaryJwk,
      {
        // these are the algorithm options
        name: 'RSASSA-PKCS1-v1_5',
        hash: { name: 'SHA-256' }, //can be "SHA-1", "SHA-256", "SHA-384", or "SHA-512"
      },
      true, //whether the key is extractable (i.e. can be used in exportKey)
      ['verify'] //"verify" for public key import, "sign" for private key imports
    );
    return publicKey;
  }

  private async exportPublicKey(publicKey: CryptoKey): Promise<string> {
    var keyData = await window.crypto.subtle.exportKey(
      'spki', // can be "jwk" (public or private), "spki" (public only), or "pkcs8" (private only)
      publicKey // can be a publicKey or privateKey, as long as extractable was true
    );
    var pubKey = this.keyDataTostring(keyData);
    return pubKey;
  }

  private str2ab(str: string) {
    const buf = new ArrayBuffer(str.length);
    const bufView = new Uint8Array(buf);
    for (let i = 0, strLen = str.length; i < strLen; i++) {
      bufView[i] = str.charCodeAt(i);
    }
    return buf;
  }

  private arrayBufferToString(buffer: ArrayBuffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    return binary;
  }

  private keyDataTostring(keydata: ArrayBuffer) {
    var keydataS = this.arrayBufferToString(keydata);
    var keydataB64 = window.btoa(keydataS);
    return keydataB64;
  }
}
