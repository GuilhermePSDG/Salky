export class Converter {
  public static BlobToBase64(
    blob: Blob,
    resultHandler: (base64: string) => void
  ): void {
    const reader = new FileReader();
    reader.onload = (event: any) => {
      resultHandler(event.target.result);
    };
    reader.readAsDataURL(blob);
  }

  public static stringToArrayBuffer(str: string): ArrayBuffer {
    var buf = new ArrayBuffer(str.length);
    var bufView = new Uint8Array(buf);
    for (var i = 0, strLen = str.length; i < strLen; i++) {
      bufView[i] = str.charCodeAt(i);
    }
    return buf;
  }

  public static arrayBufferToString(buffer: ArrayBuffer): string {
    var byteArray = new Uint8Array(buffer);
    var byteString = '';
    for (var i = 0; i < byteArray.byteLength; i++) {
      byteString += String.fromCodePoint(byteArray[i]);
    }
    return byteString;
  }
}
