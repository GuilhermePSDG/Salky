export class ListHelper {
  public static TryRemove<T>(
    source: T[],
    fieldComparer: keyof T,
    fieldValue: any
  ): T[] {
    console.log('removendo membro');
    var i = source.findIndex((x) => x[fieldComparer] == fieldValue);
    if (i !== -1) {
      source.splice(i, 1);
    }
    return source;
  }
  public static AddOrUpdate<T>(
    source: T[],
    data: T,
    fieldComparer: keyof T
  ): T[] {
    var i = source.findIndex((x) => x[fieldComparer] == data[fieldComparer]);
    if (i === -1) {
      source.push(data);
    } else {
      source[i] = data;
    }
    return source;
  }

  public static UpdateField<T>(
    source: T[],
    fieldComparer: keyof T,
    FieldComparerValue: any,
    fieldToupdate: keyof T,
    fieldToUpdateValue: any
  ): T[] {
    var i = source.findIndex((x) => x[fieldComparer] == FieldComparerValue);
    if (i === -1) {
      throw new Error('Element not found');
    } else {
      source[i][fieldToupdate] = fieldToUpdateValue;
    }
    return source;
  }
  public static Any<T>(source : T[],comparer : (data : T) => boolean){
    return source.findIndex(x => comparer(x)) !== -1;
  }
}
