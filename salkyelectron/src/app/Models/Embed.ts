export interface Embed {
  content: string;
  type: EmbedType;
}

export enum EmbedType {
  UNKNOW,
  EMOJI,
  URL,
  URL_YOUTUBE_EMBED,
  URL_GIPHY
}