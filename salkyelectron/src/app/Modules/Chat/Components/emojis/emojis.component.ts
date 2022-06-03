import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-emojis',
  templateUrl: './emojis.component.html',
  styleUrls: ['./emojis.component.scss']
})
export class EmojisComponent implements OnInit {

  @Input() isOpen = true;
  @Output() onEmojiCliked = new EventEmitter<string>();
  constructor() { }

  ngOnInit(): void
  {
  }



  public emojis = [
    {
      "Emoji": "🙂",
      "Meaning": "Slightly smiling face",
      "Unicode": "U+1F642"
    },
    {
      "Emoji": "😀",
      "Meaning": "Smiling face",
      "Unicode": "U+1F600"
    },
    {
      "Emoji": "😃",
      "Meaning": "Smiling face with big eyes",
      "Unicode": "U+1F603"
    },
    {
      "Emoji": "😄",
      "Meaning": "Smiling face with smiling eyes",
      "Unicode": "U+1F604"
    },
    {
      "Emoji": "😁",
      "Meaning": "Beaming face with smiling eyes",
      "Unicode": "U+1F601"
    },
    {
      "Emoji": "😅",
      "Meaning": "Smiling face with tears",
      "Unicode": "U+1F605"
    },
    {
      "Emoji": "😆",
      "Meaning": "Grinning face",
      "Unicode": "U+1F606"
    },
    {
      "Emoji": "🤣",
      "Meaning": "Rolling on the floor laughing",
      "Unicode": "U+1F923"
    },
    {
      "Emoji": "😂",
      "Meaning": "Lauging with tears",
      "Unicode": "U+1F602"
    },
    {
      "Emoji": "🙃",
      "Meaning": "Upside down face",
      "Unicode": "U+1F643"
    },
    {
      "Emoji": "😉",
      "Meaning": "Winking face",
      "Unicode": "U+1F609"
    },
    {
      "Emoji": "😊",
      "Meaning": "Smiling face with smiling eyes",
      "Unicode": "U+1F60A"
    },
    {
      "Emoji": "😇",
      "Meaning": "Smiling face with halo",
      "Unicode": "U+1F607"
    },
    {
      "Emoji": "😎",
      "Meaning": "Smiling face with sunglasses",
      "Unicode": "U+1F60E"
    },
    {
      "Emoji": "🤓",
      "Meaning": "Nerdy face",
      "Unicode": "U+1F913"
    },
    {
      "Emoji": "🧐",
      "Meaning": "Face with monocle",
      "Unicode": "U+1F9D0"
    },
    {
      "Emoji": "🥳",
      "Meaning": "Partying face",
      "Unicode": "U+1F973"
    },
    {
      "Emoji": "🥰",
      "Meaning": "Smiling face with hearts",
      "Unicode": "U+1F970"
    },
    {
      "Emoji": "😍",
      "Meaning": "Smiling face with heart eyes",
      "Unicode": "U+1F60D"
    },
    {
      "Emoji": "🤩",
      "Meaning": "Star-struck",
      "Unicode": "U+1F60D"
    },
    {
      "Emoji": "😘",
      "Meaning": "Face blowing kiss",
      "Unicode": "U+1F618"
    },
    {
      "Emoji": "😗",
      "Meaning": "Kissing face",
      "Unicode": "U+1F617"
    },
    {
      "Emoji": "😚",
      "Meaning": "Kissing face with closed eyes",
      "Unicode": "U+1F61A"
    },
    {
      "Emoji": "😙",
      "Meaning": "Kissng face with smiling eyes",
      "Unicode": "U+1F619"
    },
    {
      "Emoji": "🥲",
      "Meaning": "Smiling face with tears",
      "Unicode": "U+1F972"
    },
    {
      "Emoji": "😋",
      "Meaning": "Yummy face",
      "Unicode": "U+1F60B"
    },
    {
      "Emoji": "😛",
      "Meaning": "Face with tongue",
      "Unicode": "U+1F61B"
    },
    {
      "Emoji": "😜",
      "Meaning": "WInking face with tongue",
      "Unicode": "U+1F61C"
    },
    {
      "Emoji": "🤪",
      "Meaning": "Zanny face",
      "Unicode": "U+1F92A"
    },
    {
      "Emoji": "😝",
      "Meaning": "Squinting face with tongue",
      "Unicode": "U+1F61D"
    },
    {
      "Emoji": "🤑",
      "Meaning": "Money face with money tongue",
      "Unicode": "U+1F911"
    },
    {
      "Emoji": "🤗",
      "Meaning": "Hugs",
      "Unicode": "U+1F917"
    },
    {
      "Emoji": "🤭",
      "Meaning": "Face with hand in mouth",
      "Unicode": "U+1F92D"
    },
    {
      "Emoji": "🤫",
      "Meaning": "Shushing face",
      "Unicode": "U+1F92B"
    },
    {
      "Emoji": "🤔",
      "Meaning": "Thinkin face",
      "Unicode": "U+1F914"
    },
    {
      "Emoji": "😐",
      "Meaning": "Neutral face",
      "Unicode": "U+1F610"
    },
    {
      "Emoji": "🤐",
      "Meaning": "Zipped mouth",
      "Unicode": "U+1F910"
    },
    {
      "Emoji": "🤨",
      "Meaning": "Face with raised eyebrow",
      "Unicode": "U+1F928"
    },
    {
      "Emoji": "😑",
      "Meaning": "Expressionless face",
      "Unicode": "U+1F611"
    },
    {
      "Emoji": "😶",
      "Meaning": "Face with no mouth",
      "Unicode": "U+1F636"
    },
    {
      "Emoji": "😏",
      "Meaning": "Smirking face",
      "Unicode": "U+1F60F"
    },
    {
      "Emoji": "😒",
      "Meaning": "Unamused face",
      "Unicode": "U+1F612"
    },
    {
      "Emoji": "🙄",
      "Meaning": "Face with rolling eyes",
      "Unicode": "U+1F644"
    },
    {
      "Emoji": "😬",
      "Meaning": "Grimacing face",
      "Unicode": "U+1F62C"
    },
    {
      "Emoji": "😮‍💨",
      "Meaning": "Grimacing face",
      "Unicode": "U+1F4A8"
    },
    {
      "Emoji": "🤥",
      "Meaning": "Lying face",
      "Unicode": "U+1F925"
    },
    {
      "Emoji": "😪",
      "Meaning": "Sleepy face",
      "Unicode": "U+1F62A"
    },
    {
      "Emoji": "😴",
      "Meaning": "Sleeping face",
      "Unicode": "U+1F634"
    },
    {
      "Emoji": "😌",
      "Meaning": "Relieved face",
      "Unicode": "U+1F60C"
    },
    {
      "Emoji": "😔",
      "Meaning": "Pensive face",
      "Unicode": "U+1F614"
    },
    {
      "Emoji": "🤤",
      "Meaning": "Drooling face",
      "Unicode": "U+1F924"
    },
    {
      "Emoji": "😷",
      "Meaning": "Face with mask",
      "Unicode": "U+1F637"
    },
    {
      "Emoji": "🤒",
      "Meaning": "Face with thermometer",
      "Unicode": "U+1F912"
    },
    {
      "Emoji": "🤕",
      "Meaning": "Face with bandage",
      "Unicode": "U+1F915"
    },
    {
      "Emoji": "🤢",
      "Meaning": "Nauseous face",
      "Unicode": "U+1F922"
    },
    {
      "Emoji": "🤮",
      "Meaning": "Vomiting face",
      "Unicode": "U+1F92E"
    },
    {
      "Emoji": "🤧",
      "Meaning": "Sneezing face",
      "Unicode": "U+1F927"
    },
    {
      "Emoji": "🥵",
      "Meaning": "Hot face",
      "Unicode": "U+1F975"
    },
    {
      "Emoji": "🥶",
      "Meaning": "Cold face",
      "Unicode": "U+1F976"
    },
    {
      "Emoji": "🥴",
      "Meaning": "Woozy face",
      "Unicode": "U+1F974"
    },
    {
      "Emoji": "😵",
      "Meaning": "Face with crossed-out face",
      "Unicode": "U+1F635"
    },
    {
      "Emoji": "🤯",
      "Meaning": "Face with exploding head",
      "Unicode": "U+1F92F"
    },
    {
      "Emoji": "😕",
      "Meaning": "Confused face",
      "Unicode": "U+1F615"
    },
    {
      "Emoji": "😟",
      "Meaning": "Worried face",
      "Unicode": "U+1F61F"
    },
    {
      "Emoji": "🙁",
      "Meaning": "Slightly frowning face",
      "Unicode": "U+1F641"
    },
    {
      "Emoji": "☹",
      "Meaning": "Frowning face",
      "Unicode": "U+2639"
    },
    {
      "Emoji": "😮",
      "Meaning": "Face with open mouth",
      "Unicode": "U+1F62E"
    },
    {
      "Emoji": "😯",
      "Meaning": "Hushed face",
      "Unicode": "U+1F62F"
    },
    {
      "Emoji": "😲",
      "Meaning": "Astonished face",
      "Unicode": "U+1F632"
    },
    {
      "Emoji": "😳",
      "Meaning": "Flushed face",
      "Unicode": "U+1F633"
    },
    {
      "Emoji": "🥺",
      "Meaning": "Begging face",
      "Unicode": "U+1F97A"
    },
    {
      "Emoji": "😦",
      "Meaning": "Frowning face with open mouth",
      "Unicode": "U+1F626"
    },
    {
      "Emoji": "😧",
      "Meaning": "Angushed face",
      "Unicode": "U+1F627"
    },
    {
      "Emoji": "😨",
      "Meaning": "Fearful face",
      "Unicode": "U+1F628"
    },
    {
      "Emoji": "😰",
      "Meaning": "Anxious face with sweat",
      "Unicode": "U+1F630"
    },
    {
      "Emoji": "😥",
      "Meaning": "Sad but relieved face",
      "Unicode": "U+1F625"
    },
    {
      "Emoji": "😢",
      "Meaning": "Crying face",
      "Unicode": "U+1F622"
    },
    {
      "Emoji": "😭",
      "Meaning": "Loudly crying face",
      "Unicode": "U+1F62D"
    },
    {
      "Emoji": "😱",
      "Meaning": "Screaming face",
      "Unicode": "U+1F631"
    },
    {
      "Emoji": "😖",
      "Meaning": "Confounded face",
      "Unicode": "U+1F616"
    },
    {
      "Emoji": "😣",
      "Meaning": "Persevering face",
      "Unicode": "U+1F623"
    },
    {
      "Emoji": "😞",
      "Meaning": "Disapointed face",
      "Unicode": "U+1F61E"
    },
    {
      "Emoji": "😓",
      "Meaning": "Downcast face with sweat",
      "Unicode": "U+1F613"
    },
    {
      "Emoji": "😩",
      "Meaning": "Weary face",
      "Unicode": "U+1F629"
    },
    {
      "Emoji": "😫",
      "Meaning": "Tired face",
      "Unicode": "U+1F62B"
    },
    {
      "Emoji": "🥱",
      "Meaning": "Yawning face",
      "Unicode": "U+1F971"
    },
    {
      "Emoji": "😤",
      "Meaning": "Face with steam",
      "Unicode": "U+1F624"
    },
    {
      "Emoji": "😡",
      "Meaning": "Pouting face",
      "Unicode": "U+1F621"
    },
    {
      "Emoji": "😠",
      "Meaning": "Angry face",
      "Unicode": "U+1F620"
    },
    {
      "Emoji": "🤬",
      "Meaning": "Face with symbols on mouth",
      "Unicode": "U+1F92C"
    },
    {
      "Emoji": "😈",
      "Meaning": "Smiling face with horns",
      "Unicode": "U+1F608"
    },
    {
      "Emoji": "👿",
      "Meaning": "Angry face with horns",
      "Unicode": "U+1F47F"
    },
    {
      "Emoji": "💀",
      "Meaning": "Skull",
      "Unicode": "U+1F480"
    },
    {
      "Emoji": "💩",
      "Meaning": "Pile of poo",
      "Unicode": "U+1F4A9"
    },
    {
      "Emoji": "🤡",
      "Meaning": "Clown",
      "Unicode": "U+1F921"
    },
    {
      "Emoji": "👹",
      "Meaning": "Ogre",
      "Unicode": "U+1F479"
    },
    {
      "Emoji": "👺",
      "Meaning": "Goblin",
      "Unicode": "U+1F47A"
    },
    {
      "Emoji": "👻",
      "Meaning": "Ghost",
      "Unicode": "U+1F47B"
    },
    {
      "Emoji": "👽",
      "Meaning": "Alien",
      "Unicode": "U+1F47D"
    },
    {
      "Emoji": "👾",
      "Meaning": "Alien monster",
      "Unicode": "U+1F47E"
    },
    {
      "Emoji": "🤖",
      "Meaning": "Robot",
      "Unicode": "U+1F916"
    },
    {
      "Emoji": "😺",
      "Meaning": "Grinnig cat",
      "Unicode": "U+1F63A"
    },
    {
      "Emoji": "😸",
      "Meaning": "Grinning cat with smiling eyes",
      "Unicode": "U+1F638"
    },
    {
      "Emoji": "😹",
      "Meaning": "Grinning cat with tears",
      "Unicode": "U+1F639"
    },
    {
      "Emoji": "😻",
      "Meaning": "Smiling cat with heart eyes",
      "Unicode": "U+1F63B"
    },
    {
      "Emoji": "😼",
      "Meaning": "Cat with wry smile",
      "Unicode": "U+1F63C"
    },
    {
      "Emoji": "😽",
      "Meaning": "Kissing cat",
      "Unicode": "U+1F63D"
    },
    {
      "Emoji": "🙀",
      "Meaning": "Weary cat",
      "Unicode": "U+1F640"
    },
    {
      "Emoji": "😿",
      "Meaning": "Crying cat",
      "Unicode": "U+1F63F"
    },
    {
      "Emoji": "😾",
      "Meaning": "Pouting cat",
      "Unicode": "U+1F63E"
    },
    {
      "Emoji": "🙈",
      "Meaning": "See no evil monkey",
      "Unicode": "U+1F648"
    },
    {
      "Emoji": "🙉",
      "Meaning": "Hear no evil monkey",
      "Unicode": "U+1F649"
    },
    {
      "Emoji": "🙊",
      "Meaning": "Speak no evil monkey",
      "Unicode": "U+1F64A"
    },
    {
      "Emoji": "💋",
      "Meaning": "Kiss",
      "Unicode": "U+1F48B"
    },
    {
      "Emoji": "💌",
      "Meaning": "Love letter",
      "Unicode": "U+1F48C"
    },
    {
      "Emoji": "💘",
      "Meaning": "Heart with arrow",
      "Unicode": "U+1F498"
    },
    {
      "Emoji": "💝",
      "Meaning": "HEart with ribbon",
      "Unicode": "U+1F49D"
    },
    {
      "Emoji": "💖",
      "Meaning": "Sparking heart",
      "Unicode": "U+1F496"
    },
    {
      "Emoji": "💗",
      "Meaning": "Growing heart",
      "Unicode": "U+1F497"
    },
    {
      "Emoji": "💓",
      "Meaning": "Beating heart",
      "Unicode": "U+1F493"
    },
    {
      "Emoji": "💞",
      "Meaning": "Revolving heart",
      "Unicode": "U+1F49E"
    },
    {
      "Emoji": "💕",
      "Meaning": "Two hearts",
      "Unicode": "U+1F495"
    },
    {
      "Emoji": "💟",
      "Meaning": "Heart decoration",
      "Unicode": "U+1F49F"
    },
    {
      "Emoji": "❣",
      "Meaning": "Heart exclamation",
      "Unicode": "U+2763"
    },
    {
      "Emoji": "💔",
      "Meaning": "Broken heart",
      "Unicode": "U+1F494"
    },
    {
      "Emoji": "❤️‍🔥",
      "Meaning": "Heart on fire",
      "Unicode": "U+2764"
    },
    {
      "Emoji": "❤️‍🩹",
      "Meaning": "Mending heart",
      "Unicode": "U+2764"
    },
    {
      "Emoji": "❤",
      "Meaning": "Red heart",
      "Unicode": "U+2764"
    },
    {
      "Emoji": "🧡",
      "Meaning": "Orange heart",
      "Unicode": "U+1F9E1"
    },
    {
      "Emoji": "💛",
      "Meaning": "Yellow heart",
      "Unicode": "U+1F49B"
    },
    {
      "Emoji": "💚",
      "Meaning": "Green heart",
      "Unicode": "U+1F49A"
    },
    {
      "Emoji": "💙",
      "Meaning": "Blue heart",
      "Unicode": "U+1F499"
    },
    {
      "Emoji": "💜",
      "Meaning": "Purple heart",
      "Unicode": "U+1F49C"
    },
    {
      "Emoji": "🤎",
      "Meaning": "Brown heart",
      "Unicode": "U+1F90E"
    },
    {
      "Emoji": "🖤",
      "Meaning": "Black heart",
      "Unicode": "U+1F5A4"
    },
    {
      "Emoji": "🤍",
      "Meaning": "White heart",
      "Unicode": "U+1F90D"
    },
    {
      "Emoji": "💯",
      "Meaning": "Hundred(correct)",
      "Unicode": "U+1F4AF"
    },
    {
      "Emoji": "💢",
      "Meaning": "Anger",
      "Unicode": "U+1F4A2"
    },
    {
      "Emoji": "💥",
      "Meaning": "collision",
      "Unicode": "U+1F4A5"
    },
    {
      "Emoji": "💫",
      "Meaning": "Dizzy",
      "Unicode": "U+1F4AB"
    },
    {
      "Emoji": "💦",
      "Meaning": "Sweat droplets",
      "Unicode": "U+1F4A6"
    },
    {
      "Emoji": "💨",
      "Meaning": "Dashing away",
      "Unicode": "U+1F4A8"
    },
    {
      "Emoji": "🕳",
      "Meaning": "Hole",
      "Unicode": "U+1F573"
    },
    {
      "Emoji": "💣",
      "Meaning": "Bomb",
      "Unicode": "U+1F4A3"
    },
    {
      "Emoji": "💬",
      "Meaning": "Message baloon",
      "Unicode": "U+1F4AC"
    },
    {
      "Emoji": "👁️‍🗨️",
      "Meaning": "Eye in speech bubble",
      "Unicode": "U+1F441"
    },
    {
      "Emoji": "🗨",
      "Meaning": "Left speech bubble",
      "Unicode": "U+1F5E8"
    },
    {
      "Emoji": "🗯",
      "Meaning": "Anger bubble",
      "Unicode": "U+1F5EF"
    },
    {
      "Emoji": "💭",
      "Meaning": "Thought baloon",
      "Unicode": "U+1F4AD"
    },
    {
      "Emoji": "💤",
      "Meaning": "zzz",
      "Unicode": "U+1F4A4"
    },
    {
      "Emoji": "👋",
      "Meaning": "Waving hand",
      "Unicode": "U+1F44B"
    },
    {
      "Emoji": "🤚",
      "Meaning": "Raised back of hand",
      "Unicode": "U+1F91A"
    },
    {
      "Emoji": "🖐",
      "Meaning": "Hands with splayed finger",
      "Unicode": "U+1F590"
    },
    {
      "Emoji": "✋",
      "Meaning": "Raised hand",
      "Unicode": "U+270B"
    },
    {
      "Emoji": "🖖",
      "Meaning": "Vulcan salute",
      "Unicode": "U+1F596"
    },
    {
      "Emoji": "👌",
      "Meaning": "Ok",
      "Unicode": "U+1F44C"
    },
    {
      "Emoji": "🤌",
      "Meaning": "Pinched fingers",
      "Unicode": "U+1F90C"
    },
    {
      "Emoji": "🤏",
      "Meaning": "Pinched hand",
      "Unicode": "U+1F90F"
    },
    {
      "Emoji": "✌",
      "Meaning": "Victory hand",
      "Unicode": "U+270C"
    },
    {
      "Emoji": "🤞",
      "Meaning": "Crossed fingers",
      "Unicode": "U+1F91E"
    },
    {
      "Emoji": "🤟",
      "Meaning": "Love you",
      "Unicode": "U+1F91F"
    },
    {
      "Emoji": "🤘",
      "Meaning": "Horn sign",
      "Unicode": "U+1F918"
    },
    {
      "Emoji": "🤙",
      "Meaning": "Call me hand",
      "Unicode": "U+1F919"
    },
    {
      "Emoji": "👈",
      "Meaning": "Index finger pointing left",
      "Unicode": "U+1F448"
    },
    {
      "Emoji": "👉",
      "Meaning": "Index finger pointing right",
      "Unicode": "U+1F449"
    },
    {
      "Emoji": "👆",
      "Meaning": "Index finger pointing up",
      "Unicode": "U+1F446"
    },
    {
      "Emoji": "👇",
      "Meaning": "Index finger pointing down",
      "Unicode": "U+1F447"
    },
    {
      "Emoji": "🖕",
      "Meaning": "Middle finger",
      "Unicode": "U+1F595"
    },
    {
      "Emoji": "☝",
      "Meaning": "Forehand Index finger pointing up",
      "Unicode": "U+261D"
    },
    {
      "Emoji": "🫵",
      "Meaning": "Index finger pointing at viewer",
      "Unicode": "U+1FAF5"
    },
    {
      "Emoji": "👍",
      "Meaning": "Thumbs up",
      "Unicode": "U+1F44D"
    },
    {
      "Emoji": "👎",
      "Meaning": "Thumbs down",
      "Unicode": "U+1F44E"
    },
    {
      "Emoji": "✊",
      "Meaning": "Raised fist",
      "Unicode": "U+270A"
    },
    {
      "Emoji": "👊",
      "Meaning": "Fist",
      "Unicode": "U+1F44A"
    },
    {
      "Emoji": "🤛",
      "Meaning": "Left facing fist",
      "Unicode": "U+1F91B"
    },
    {
      "Emoji": "🤜",
      "Meaning": "Right facing fist",
      "Unicode": "U+1F91C"
    },
    {
      "Emoji": "👏",
      "Meaning": "Clapping hands",
      "Unicode": "U+1F44F"
    },
    {
      "Emoji": "🙌",
      "Meaning": "Raised hands",
      "Unicode": "U+1F64C"
    },
    {
      "Emoji": "👐",
      "Meaning": "OPen hands",
      "Unicode": "U+1F450"
    },
    {
      "Emoji": "🤲",
      "Meaning": "Palms together hands",
      "Unicode": "U+1F932"
    },
    {
      "Emoji": "🤝",
      "Meaning": "Handshake",
      "Unicode": "U+1F91D"
    },
    {
      "Emoji": "🙏",
      "Meaning": "Praying hands",
      "Unicode": "U+1F64F"
    },
    {
      "Emoji": "✍",
      "Meaning": "Writing hands",
      "Unicode": "U+270D"
    },
    {
      "Emoji": "💅",
      "Meaning": "Nail polish",
      "Unicode": "U+1F485"
    },
    {
      "Emoji": "🤳",
      "Meaning": "Selfie hand",
      "Unicode": "U+1F933"
    },
    {
      "Emoji": "💪",
      "Meaning": "Flexed biceps",
      "Unicode": "U+1F4AA"
    },
    {
      "Emoji": "🦾",
      "Meaning": "MEchanical arm",
      "Unicode": "U+1F9BE"
    },
    {
      "Emoji": "🦵",
      "Meaning": "Leg",
      "Unicode": "U+1F9B5"
    },
    {
      "Emoji": "🦿",
      "Meaning": "Mechanical leg",
      "Unicode": "U+1F9BF"
    },
    {
      "Emoji": "🦶",
      "Meaning": "Foot",
      "Unicode": "U+1F9B6"
    },
    {
      "Emoji": "👂",
      "Meaning": "Ear",
      "Unicode": "U+1F442"
    },
    {
      "Emoji": "🦻",
      "Meaning": "Ear with earing aid",
      "Unicode": "U+1F9BB"
    },
    {
      "Emoji": "👃",
      "Meaning": "Nose",
      "Unicode": "U+1F443"
    },
    {
      "Emoji": "🧠",
      "Meaning": "Brain",
      "Unicode": "U+1F9E0"
    },
    {
      "Emoji": "👣",
      "Meaning": "Footprint",
      "Unicode": "U+1F463"
    },
    {
      "Emoji": "🫀",
      "Meaning": "MEchanical heart",
      "Unicode": "U+1FAC0"
    },
    {
      "Emoji": "🫁",
      "Meaning": "Lungs",
      "Unicode": "U+1FAC1"
    },
    {
      "Emoji": "🦷",
      "Meaning": "Tooth",
      "Unicode": "U+1F9B7"
    },
    {
      "Emoji": "🦴",
      "Meaning": "Bone",
      "Unicode": "U+1F9B7"
    },
    {
      "Emoji": "👀",
      "Meaning": "Eyes",
      "Unicode": "U+1F440"
    },
    {
      "Emoji": "👁",
      "Meaning": "Eye",
      "Unicode": "U+1F441"
    },
    {
      "Emoji": "👅",
      "Meaning": "Nose",
      "Unicode": "U+1F445"
    },
    {
      "Emoji": "👄",
      "Meaning": "Mouth",
      "Unicode": "U+1F444"
    },
    {
      "Emoji": "🧑",
      "Meaning": "Person",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👶",
      "Meaning": "Baby",
      "Unicode": "U+1F476"
    },
    {
      "Emoji": "🧒",
      "Meaning": "Child",
      "Unicode": "U+1F9D2"
    },
    {
      "Emoji": "👦",
      "Meaning": "Boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👧",
      "Meaning": "Girl",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👱",
      "Meaning": "Person with blonde hair",
      "Unicode": "U+1F471"
    },
    {
      "Emoji": "👨",
      "Meaning": "Man",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "🧔",
      "Meaning": "Bearded person",
      "Unicode": "U+1F9D4"
    },
    {
      "Emoji": "🧔‍♂‍",
      "Meaning": "Bearded man",
      "Unicode": "U+1F9D4"
    },
    {
      "Emoji": "🧔‍♀‍",
      "Meaning": "Bearded woman",
      "Unicode": "U+1F9D4"
    },
    {
      "Emoji": "👨‍🦰",
      "Meaning": "MAn with red hair",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👨‍🦱",
      "Meaning": "Man with curly hair",
      "Unicode": "U+1F9B1"
    },
    {
      "Emoji": "👨‍🦳",
      "Meaning": "Man with white hair",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍🦲",
      "Meaning": "Bald man",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩",
      "Meaning": "Woman",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👩‍🦰",
      "Meaning": "Woman with red hair",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👩‍🦱",
      "Meaning": "Woman with curly hair",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👩‍🦳",
      "Meaning": "Woman with white hair",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👩‍🦲",
      "Meaning": "Bald woman",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👱‍♀‍",
      "Meaning": "Woman with blode hair",
      "Unicode": "U+1F471"
    },
    {
      "Emoji": "👱‍♂‍",
      "Meaning": "Man with blonde hair",
      "Unicode": "U+1F471"
    },
    {
      "Emoji": "🧓",
      "Meaning": "Old person",
      "Unicode": "U+1F9D3"
    },
    {
      "Emoji": "👴",
      "Meaning": "Old man",
      "Unicode": "U+1F474"
    },
    {
      "Emoji": "👵",
      "Meaning": "Old woman",
      "Unicode": "U+1F475"
    },
    {
      "Emoji": "🙍",
      "Meaning": "Person frowning",
      "Unicode": "U+1F64D"
    },
    {
      "Emoji": "🙍‍♂‍",
      "Meaning": "Man frowning",
      "Unicode": "U+1F64D"
    },
    {
      "Emoji": "🙍‍♀‍",
      "Meaning": "Woman frowning",
      "Unicode": "U+1F64D"
    },
    {
      "Emoji": "🙎",
      "Meaning": "Person pouting",
      "Unicode": "U+1F64E"
    },
    {
      "Emoji": "🙎‍♂‍",
      "Meaning": "Man pouting",
      "Unicode": "U+1F64E"
    },
    {
      "Emoji": "🙎‍♀‍",
      "Meaning": "Woman pouting",
      "Unicode": "U+1F64E"
    },
    {
      "Emoji": "🙅",
      "Meaning": "Person gesturing no",
      "Unicode": "U+1F645"
    },
    {
      "Emoji": "🙅‍♂‍",
      "Meaning": "Man gesturing no",
      "Unicode": "U+1F645"
    },
    {
      "Emoji": "🙅‍♀‍",
      "Meaning": "Woman gesturing no",
      "Unicode": "U+1F645"
    },
    {
      "Emoji": "🙆",
      "Meaning": "Person stretching",
      "Unicode": "U+1F646"
    },
    {
      "Emoji": "🙆‍♂‍",
      "Meaning": "Man stretching",
      "Unicode": "U+1F646"
    },
    {
      "Emoji": "🙆‍♀‍",
      "Meaning": "Woman stretching",
      "Unicode": "U+1F646"
    },
    {
      "Emoji": "💁",
      "Meaning": "Person tipping hand",
      "Unicode": "U+1F481"
    },
    {
      "Emoji": "💁‍♂‍",
      "Meaning": "Man tipping hand",
      "Unicode": "U+1F481"
    },
    {
      "Emoji": "💁‍♀‍",
      "Meaning": "Woman tipping hand",
      "Unicode": "U+1F481"
    },
    {
      "Emoji": "🙋",
      "Meaning": "Person rainsing hand",
      "Unicode": "U+1F64B"
    },
    {
      "Emoji": "🙋‍♂‍",
      "Meaning": "Man raising hand",
      "Unicode": "U+1F64B"
    },
    {
      "Emoji": "🙋‍♀‍",
      "Meaning": "Woman raisning hand",
      "Unicode": "U+1F64B"
    },
    {
      "Emoji": "🧏",
      "Meaning": "Deaf person",
      "Unicode": "U+1F64B"
    },
    {
      "Emoji": "🧏‍♂‍",
      "Meaning": "Deaf man",
      "Unicode": "U+1F9CF"
    },
    {
      "Emoji": "🧏‍♀‍",
      "Meaning": "Deaf woman",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🙇",
      "Meaning": "Person bowing",
      "Unicode": "U+1F647"
    },
    {
      "Emoji": "🙇‍♂‍",
      "Meaning": "Man bowing",
      "Unicode": "U+1F647"
    },
    {
      "Emoji": "🙇‍♀‍",
      "Meaning": "Woman bowing",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤦",
      "Meaning": "Person facepalming",
      "Unicode": "U+1F926"
    },
    {
      "Emoji": "🤦‍♂‍",
      "Meaning": "Man facepalming",
      "Unicode": "U+1F926"
    },
    {
      "Emoji": "🤦‍♀‍",
      "Meaning": "Woman facepalming",
      "Unicode": "U+1F926"
    },
    {
      "Emoji": "🤷",
      "Meaning": "Person shrugging",
      "Unicode": "U+1F937"
    },
    {
      "Emoji": "🤷‍♂‍",
      "Meaning": "Man shrugging",
      "Unicode": "U+1F937"
    },
    {
      "Emoji": "🤷‍♀‍",
      "Meaning": "Woman shrugging",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧑‍⚕‍",
      "Meaning": "Health worker",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍⚕‍",
      "Meaning": "Man health worker",
      "Unicode": "U+2695"
    },
    {
      "Emoji": "👩‍⚕‍",
      "Meaning": "Woman health worker",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🎓",
      "Meaning": "Student",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🎓",
      "Meaning": "Man student",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "🧑‍🏫",
      "Meaning": "Teacher",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🏫",
      "Meaning": "Man teacher",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🏫",
      "Meaning": "Woman teacher",
      "Unicode": "U+1F3EB"
    },
    {
      "Emoji": "🧑‍⚖‍",
      "Meaning": "Judge",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍⚖‍",
      "Meaning": "Man judge",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍⚖‍",
      "Meaning": "Woman judge",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🌾",
      "Meaning": "Farmer",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍🌾",
      "Meaning": "Man farmer",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🌾",
      "Meaning": "Woman farmer",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🍳",
      "Meaning": "Cook",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🍳",
      "Meaning": "Man cook",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👩‍🍳",
      "Meaning": "Woman cook",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🔧",
      "Meaning": "Mechanic",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🔧",
      "Meaning": "Man mechanic",
      "Unicode": "U+1F527"
    },
    {
      "Emoji": "👩‍🔧",
      "Meaning": "Woman mechanic",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🏭",
      "Meaning": "Factory worker",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🏭",
      "Meaning": "Man factory worker",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🏭",
      "Meaning": "Woman factory worker",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍💼",
      "Meaning": "Office worker",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍💼",
      "Meaning": "Man office worker",
      "Unicode": "U+1F4BC"
    },
    {
      "Emoji": "👩‍💼",
      "Meaning": "Woma office worker",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🔬",
      "Meaning": "Scientist",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🔬",
      "Meaning": "Man scientist",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🔬",
      "Meaning": "Woman scientis",
      "Unicode": "U+1F52C"
    },
    {
      "Emoji": "🧑‍💻",
      "Meaning": "Technologist",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍💻",
      "Meaning": "Man texhnologist",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍💻",
      "Meaning": "Woman technologist",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🎤",
      "Meaning": "Singer",
      "Unicode": "U+1F3A4"
    },
    {
      "Emoji": "👨‍🎤",
      "Meaning": "Man singer",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🎤",
      "Meaning": "Woman singer",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🎨",
      "Meaning": "Artist",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🎨",
      "Meaning": "Man artist",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🎨",
      "Meaning": "Woman artist",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍✈‍",
      "Meaning": "Pilot",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍✈‍",
      "Meaning": "Man pilot",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍✈‍",
      "Meaning": "Woman pilot",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🚀",
      "Meaning": "Astronaut",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🚀",
      "Meaning": "Man astronaut",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🚀",
      "Meaning": "Woman astronaut",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "🧑‍🚒",
      "Meaning": "Firefighter",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🚒",
      "Meaning": "Man firefighter",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👩‍🚒",
      "Meaning": "Woman firefighter",
      "Unicode": "U+1F692"
    },
    {
      "Emoji": "👮",
      "Meaning": "Police",
      "Unicode": "U+1F46E"
    },
    {
      "Emoji": "👮‍♂‍",
      "Meaning": "Policeman",
      "Unicode": "U+1F46E"
    },
    {
      "Emoji": "👮‍♀‍",
      "Meaning": "Policewoman",
      "Unicode": "U+1F46E"
    },
    {
      "Emoji": "🕵",
      "Meaning": "Detective",
      "Unicode": "U+1F575"
    },
    {
      "Emoji": "🕵️‍♂‍",
      "Meaning": "Man detective",
      "Unicode": "U+1F575"
    },
    {
      "Emoji": "🕵️‍♀‍",
      "Meaning": "Woman detective",
      "Unicode": "U+1F575"
    },
    {
      "Emoji": "💂",
      "Meaning": "Guard",
      "Unicode": "U+1F482"
    },
    {
      "Emoji": "💂‍♂‍",
      "Meaning": "Man guard",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "💂‍♀‍",
      "Meaning": "Woman guard",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🥷",
      "Meaning": "Ninja",
      "Unicode": "U+1F97"
    },
    {
      "Emoji": "👷",
      "Meaning": "Construction worker",
      "Unicode": "U+1F477"
    },
    {
      "Emoji": "👷‍♂‍",
      "Meaning": "Man construction worker",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👷‍♀‍",
      "Meaning": "Woman construction worker",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🤴",
      "Meaning": "Prince",
      "Unicode": "U+1F934"
    },
    {
      "Emoji": "👸",
      "Meaning": "Princess",
      "Unicode": "U+1F478"
    },
    {
      "Emoji": "👳",
      "Meaning": "Person wearing turban",
      "Unicode": "U+1F473"
    },
    {
      "Emoji": "👳‍♂‍",
      "Meaning": "Man wearing turban",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👳‍♀‍",
      "Meaning": "Woman wearing turban",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "👲",
      "Meaning": "Person with skullcap",
      "Unicode": "U+1F472"
    },
    {
      "Emoji": "🧕",
      "Meaning": "Woman with headscaff",
      "Unicode": "U+1F9D5"
    },
    {
      "Emoji": "🤵",
      "Meaning": "Person in tuxedo",
      "Unicode": "U+1F935"
    },
    {
      "Emoji": "🤵‍♂‍",
      "Meaning": "Man in tuxedo",
      "Unicode": "U+1F935"
    },
    {
      "Emoji": "🤵‍♀‍",
      "Meaning": "Woman in tuxedo",
      "Unicode": "U+1F935"
    },
    {
      "Emoji": "👰",
      "Meaning": "Person in veil",
      "Unicode": "U+1F470"
    },
    {
      "Emoji": "👰‍♂‍",
      "Meaning": "Man in veil",
      "Unicode": "U+1F470"
    },
    {
      "Emoji": "👰‍♀‍",
      "Meaning": "Woman in veil",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤰",
      "Meaning": "Pregnant woman",
      "Unicode": "U+1F930"
    },
    {
      "Emoji": "🤱",
      "Meaning": "Breast-feeding",
      "Unicode": "U+1F931"
    },
    {
      "Emoji": "🧑‍🍼",
      "Meaning": "Person feeding baby",
      "Unicode": "U+1F37C"
    },
    {
      "Emoji": "👩‍🍼",
      "Meaning": "WOman feeding baby",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👨‍🍼",
      "Meaning": "Man feeding baby",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👼",
      "Meaning": "Baby angel",
      "Unicode": "U+1F47C"
    },
    {
      "Emoji": "🎅",
      "Meaning": "Santa claus",
      "Unicode": "U+1F385"
    },
    {
      "Emoji": "🤶",
      "Meaning": "Mrs Claus",
      "Unicode": "U+1F936"
    },
    {
      "Emoji": "🧑‍🎄",
      "Meaning": "Mx Claus",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "🦸",
      "Meaning": "Superhero",
      "Unicode": "U+1F9B8"
    },
    {
      "Emoji": "🦸‍♂‍",
      "Meaning": "Man superhero",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🦸‍♀‍",
      "Meaning": "Woman superhero",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🦹",
      "Meaning": "Supervillain",
      "Unicode": "U+1F9B9"
    },
    {
      "Emoji": "🦹‍♂‍",
      "Meaning": "Man superhero",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🦹‍♀‍",
      "Meaning": "Woman superhero",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧙",
      "Meaning": "Mage",
      "Unicode": "U+1F9D9"
    },
    {
      "Emoji": "🧙‍♂‍",
      "Meaning": "Man mage",
      "Unicode": "U+1F9D9"
    },
    {
      "Emoji": "🧙‍♀‍",
      "Meaning": "Woman mage",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🧚",
      "Meaning": "Fairy",
      "Unicode": "U+1F9DA"
    },
    {
      "Emoji": "🧚‍♂‍",
      "Meaning": "Man fairy",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🧚‍♀‍",
      "Meaning": "Woman fairy",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧛",
      "Meaning": "Vampire",
      "Unicode": "U+1F9DB"
    },
    {
      "Emoji": "🧛‍♂‍",
      "Meaning": "Man vampire",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🧛‍♀‍",
      "Meaning": "Woman vampire",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧜",
      "Meaning": "Merperson",
      "Unicode": "U+1F9DC"
    },
    {
      "Emoji": "🧜‍♂‍",
      "Meaning": "Merman",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧜‍♀‍",
      "Meaning": "Mermaid",
      "Unicode": "U+1F9DC"
    },
    {
      "Emoji": "🧝",
      "Meaning": "Elf",
      "Unicode": "U+1F9DD"
    },
    {
      "Emoji": "🧝‍♂‍",
      "Meaning": "Man elf",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🧝‍♀‍",
      "Meaning": "Woman elf",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧞",
      "Meaning": "Genie",
      "Unicode": "U+1F9DE"
    },
    {
      "Emoji": "🧞‍♂‍",
      "Meaning": "Man genie",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧞‍♀‍",
      "Meaning": "Woman genie",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🧟",
      "Meaning": "Zombie",
      "Unicode": "U+1F9DF"
    },
    {
      "Emoji": "🧟‍♂‍",
      "Meaning": "Man zombie",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🧟‍♀‍",
      "Meaning": "Woman zombie",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "💆",
      "Meaning": "Person getting massage",
      "Unicode": "U+1F486"
    },
    {
      "Emoji": "💆‍♂‍",
      "Meaning": "Man getting massage",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "💆‍♀‍",
      "Meaning": "Woman getting massage",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "💇",
      "Meaning": "Person getting haircut",
      "Unicode": "U+1F487"
    },
    {
      "Emoji": "💇‍♂‍",
      "Meaning": "Man getting haircut",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "💇‍♀‍",
      "Meaning": "Woman getting haircut",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🚶",
      "Meaning": "Person walking",
      "Unicode": "U+1F6B6"
    },
    {
      "Emoji": "🚶‍♂‍",
      "Meaning": "Man walking",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🚶‍♀‍",
      "Meaning": "Woman walking",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧍",
      "Meaning": "Person standing",
      "Unicode": "U+1F9CD"
    },
    {
      "Emoji": "🧍‍♂‍",
      "Meaning": "Man standing",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧍‍♀‍",
      "Meaning": "Woman standing",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧎",
      "Meaning": "Person kneeling",
      "Unicode": "U+1F9CE"
    },
    {
      "Emoji": "🧎‍♂‍",
      "Meaning": "Man kneeling",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧎‍♀‍",
      "Meaning": "Woman kneeling",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧑‍🦯",
      "Meaning": "PErson with walking stick",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍🦯",
      "Meaning": "Man with walking stick",
      "Unicode": "U+1F468"
    },
    {
      "Emoji": "👩‍🦯",
      "Meaning": "Woman with walking stick",
      "Unicode": "U+1F9AF"
    },
    {
      "Emoji": "🧑‍🦼",
      "Meaning": "Person in motorized wheelchair",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🦼",
      "Meaning": "Man in motorized wheelchair",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👩‍🦼",
      "Meaning": "Womain in motorized wheelchair",
      "Unicode": "U+1F9BC"
    },
    {
      "Emoji": "🧑‍🦽",
      "Meaning": "Person in manual wheelchair",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👨‍🦽",
      "Meaning": "Man in manual wheelchair",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👩‍🦽",
      "Meaning": "Womaan in motorized wheelchair",
      "Unicode": "U+1F9BD"
    },
    {
      "Emoji": "🏃",
      "Meaning": "Person sprinting",
      "Unicode": "U+1F3C3"
    },
    {
      "Emoji": "🏃‍♂‍",
      "Meaning": "Man sprinting",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🏃‍♀‍",
      "Meaning": "Woman sprinting",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "💃",
      "Meaning": "Woman dancing",
      "Unicode": "U+1F483"
    },
    {
      "Emoji": "🕺",
      "Meaning": "Man dancing",
      "Unicode": "U+1F57A"
    },
    {
      "Emoji": "🕴",
      "Meaning": "Person in suit levitating",
      "Unicode": "U+1F574"
    },
    {
      "Emoji": "👯",
      "Meaning": "People with bunny ears",
      "Unicode": "U+1F46F"
    },
    {
      "Emoji": "👯‍♂‍",
      "Meaning": "Men with bunny ears",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👯‍♀‍",
      "Meaning": "Women in bunny ears",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🧖",
      "Meaning": "Person in steaming room",
      "Unicode": "U+1F9D6"
    },
    {
      "Emoji": "🧖‍♂‍",
      "Meaning": "Man in steaming room",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧖‍♀‍",
      "Meaning": "Woman in steaming room",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧗",
      "Meaning": "Person climbing",
      "Unicode": "U+1F9D7"
    },
    {
      "Emoji": "🧗‍♂‍",
      "Meaning": "Man climbing",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧗‍♀‍",
      "Meaning": "Woman climbing",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🤺",
      "Meaning": "Person fencing",
      "Unicode": "U+1F93A"
    },
    {
      "Emoji": "🏇",
      "Meaning": "Horse racing",
      "Unicode": "U+1F3C7"
    },
    {
      "Emoji": "⛷",
      "Meaning": "Skier",
      "Unicode": "U+26F7"
    },
    {
      "Emoji": "🏂",
      "Meaning": "Snowball",
      "Unicode": "U+1F3C2"
    },
    {
      "Emoji": "🏌",
      "Meaning": "Person playing golf",
      "Unicode": "U+1F3CC"
    },
    {
      "Emoji": "🏌️‍♂‍",
      "Meaning": "Man playing golf",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🏌️‍♀‍",
      "Meaning": "Woman playing golf",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🏄",
      "Meaning": "Person surfing",
      "Unicode": "U+1F3C4"
    },
    {
      "Emoji": "🏄‍♂‍",
      "Meaning": "Man surfing",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🏄‍♀‍",
      "Meaning": "Woman surfing",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🚣",
      "Meaning": "Person rowing boat",
      "Unicode": "U+1F6A3"
    },
    {
      "Emoji": "🚣‍♂‍",
      "Meaning": "Man rowing boat",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🚣‍♀‍",
      "Meaning": "Woman rowing boat",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🏊",
      "Meaning": "Person swimming",
      "Unicode": "U+1F3CA"
    },
    {
      "Emoji": "🏊‍♂‍",
      "Meaning": "Man swimming",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🏊‍♀‍",
      "Meaning": "Woman swimming",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "⛹",
      "Meaning": "Person bouncing ball",
      "Unicode": "U+26F9"
    },
    {
      "Emoji": "⛹️‍♂‍",
      "Meaning": "Man bouncing ball",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "⛹️‍♀‍",
      "Meaning": "Woman bouncing ball",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🏋",
      "Meaning": "Person lifting weight",
      "Unicode": "U+1F3CB"
    },
    {
      "Emoji": "🏋️‍♂‍",
      "Meaning": "Man lifting weight",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🏋️‍♀‍",
      "Meaning": "Woman lifting weight",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🚴",
      "Meaning": "Person cycling",
      "Unicode": "U+1F6B4"
    },
    {
      "Emoji": "🚴‍♂‍",
      "Meaning": "Man cycling",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🚴‍♀‍",
      "Meaning": "Woman cycling",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🚵",
      "Meaning": "Person mountain biking",
      "Unicode": "U+1F6B5"
    },
    {
      "Emoji": "🚵‍♂‍",
      "Meaning": "Man mountain biking",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "🚵‍♀‍",
      "Meaning": "Woman mountain biking",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🤸",
      "Meaning": "Person catwheeling",
      "Unicode": "U+1F938"
    },
    {
      "Emoji": "🤸‍♂‍",
      "Meaning": "Man catwheeling",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤸‍♀‍",
      "Meaning": "Woman catwheeling",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🤼",
      "Meaning": "People wrestling",
      "Unicode": "U+1F93C"
    },
    {
      "Emoji": "🤼‍♂‍",
      "Meaning": "Men wrestling",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤼‍♀‍",
      "Meaning": "Women wrestling",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🤽",
      "Meaning": "Person playing water polo",
      "Unicode": "U+1F93D"
    },
    {
      "Emoji": "🤽‍♂‍",
      "Meaning": "Man playing water polo",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🤽‍♀‍",
      "Meaning": "Woman playing water polo",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤾",
      "Meaning": "Person playing handball",
      "Unicode": "U+1F93E"
    },
    {
      "Emoji": "🤾‍♂‍",
      "Meaning": "Man playing handball",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤾‍♀‍",
      "Meaning": "Woman playing handball",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🤹",
      "Meaning": "Person juggling",
      "Unicode": "U+1F939"
    },
    {
      "Emoji": "🤹‍♂‍",
      "Meaning": "Man juggling",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🤹‍♀‍",
      "Meaning": "Woman juggling",
      "Unicode": "U+FE0F"
    },
    {
      "Emoji": "🧘",
      "Meaning": "Person lotus position",
      "Unicode": "U+1F9D8"
    },
    {
      "Emoji": "🧘‍♂‍",
      "Meaning": "Man in lotus position",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🧘‍♀‍",
      "Meaning": "Woman in lotus position",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "🛀",
      "Meaning": "Person bathing",
      "Unicode": "U+1F6C0"
    },
    {
      "Emoji": "🛌",
      "Meaning": "Person in bed",
      "Unicode": "U+1F6CC"
    },
    {
      "Emoji": "👪",
      "Meaning": "Family",
      "Unicode": "U+1F46A"
    },
    {
      "Emoji": "👨‍👩‍👦",
      "Meaning": "Family: man, woman, and boy",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍👩‍👧",
      "Meaning": "Family: man, woman, and girl",
      "Unicode": "U+1F469"
    },
    {
      "Emoji": "👨‍👩‍👧‍👦",
      "Meaning": "Family: man, woman, boy, and girl",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍👩‍👦‍👦",
      "Meaning": "Family: man, woman, and two boys",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👩‍👧‍👧",
      "Meaning": "Family: man, woman, and two girls",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍👨‍👦",
      "Meaning": "Family: two men and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👨‍👧",
      "Meaning": "Family: two men and girl",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍👨‍👧‍👦",
      "Meaning": "Family: two men, girl, and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👨‍👦‍👦",
      "Meaning": "Family: two men and two boys",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍👨‍👧‍👧",
      "Meaning": "Family: two men and two girls",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👩‍👩‍👦",
      "Meaning": "Family: two women and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👩‍👧",
      "Meaning": "Family: two women and girl",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👩‍👩‍👧‍👦",
      "Meaning": "Family: two women, girl, and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👩‍👦‍👦",
      "Meaning": "Family: two women and two boys",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👩‍👧‍👧",
      "Meaning": "Family: two women and two girls",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👨‍👦",
      "Meaning": "Family: man and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👦‍👦",
      "Meaning": "Family: man and two boys",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👧",
      "Meaning": "Family: man and girl",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👨‍👧‍👦",
      "Meaning": "Family: man, girl and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👨‍👧‍👧",
      "Meaning": "Family: man and two girls",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👩‍👦",
      "Meaning": "Family: woman and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👦‍👦",
      "Meaning": "Family: woman and two boys",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👧",
      "Meaning": "Family: woman and girl",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "👩‍👧‍👦",
      "Meaning": "Family: woman, girl, and boy",
      "Unicode": "U+1F466"
    },
    {
      "Emoji": "👩‍👧‍👧",
      "Meaning": "Family: woman and two girls",
      "Unicode": "U+1F467"
    },
    {
      "Emoji": "🧑‍🤝‍🧑",
      "Meaning": "People holding hands",
      "Unicode": "U+1F9D1"
    },
    {
      "Emoji": "👭",
      "Meaning": "Women holding hands",
      "Unicode": "U+1F46D"
    },
    {
      "Emoji": "👫",
      "Meaning": "Woman and man holding hands",
      "Unicode": "U+1F46B"
    },
    {
      "Emoji": "👬",
      "Meaning": "Men holding hands",
      "Unicode": "U+1F46C"
    },
    {
      "Emoji": "💏",
      "Meaning": "Kiss",
      "Unicode": "U+1F48F"
    },
    {
      "Emoji": "👩‍❤‍💋‍👨",
      "Meaning": "Woman and man kissing",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👨‍❤‍💋‍👨",
      "Meaning": "Man and man kissing",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "👩‍❤‍💋‍👩",
      "Meaning": "Womand and woman kissing",
      "Unicode": "U+1F48B"
    },
    {
      "Emoji": "💑",
      "Meaning": "Couple with heart",
      "Unicode": "U+1F491"
    },
    {
      "Emoji": "🗣",
      "Meaning": "Person speaking",
      "Unicode": "U+1F5E3"
    },
    {
      "Emoji": "👤",
      "Meaning": "Bust in silhouhette",
      "Unicode": "U+1F464"
    },
    {
      "Emoji": "👥",
      "Meaning": "Busts in silhouette",
      "Unicode": "U+1F465"
    },
    {
      "Emoji": "🫂",
      "Meaning": "People hugging",
      "Unicode": "U+1FAC2"
    },
    {
      "Emoji": "🐵",
      "Meaning": "Monkey face",
      "Unicode": "U+1F435"
    },
    {
      "Emoji": "🐒",
      "Meaning": "Monkey",
      "Unicode": "U+1F412"
    },
    {
      "Emoji": "🦍",
      "Meaning": "Gorilla",
      "Unicode": "U+1F98D"
    },
    {
      "Emoji": "🦧",
      "Meaning": "Orangutan",
      "Unicode": "U+1F9A7"
    },
    {
      "Emoji": "🐶",
      "Meaning": "Dog face",
      "Unicode": "U+1F436"
    },
    {
      "Emoji": "🐕",
      "Meaning": "Dog",
      "Unicode": "U+1F415"
    },
    {
      "Emoji": "🦮",
      "Meaning": "Guide dog",
      "Unicode": "U+1F9AE"
    },
    {
      "Emoji": "🐕‍🦺",
      "Meaning": "Service dog",
      "Unicode": "U+1F415"
    },
    {
      "Emoji": "🐩",
      "Meaning": "Poodle",
      "Unicode": "U+1F429"
    },
    {
      "Emoji": "🐺",
      "Meaning": "Wolf",
      "Unicode": "U+1F43A"
    },
    {
      "Emoji": "🦊",
      "Meaning": "Fox",
      "Unicode": "U+1F98A"
    },
    {
      "Emoji": "🦝",
      "Meaning": "Racoon",
      "Unicode": "U+1F99D"
    },
    {
      "Emoji": "🐱",
      "Meaning": "Cat face",
      "Unicode": "U+1F431"
    },
    {
      "Emoji": "🐈",
      "Meaning": "Cat",
      "Unicode": "U+1F408"
    },
    {
      "Emoji": "🐈‍⬛",
      "Meaning": "Black cat",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🦁",
      "Meaning": "Lion",
      "Unicode": "U+1F981"
    },
    {
      "Emoji": "🐯",
      "Meaning": "Tiger face",
      "Unicode": "U+1F42F"
    },
    {
      "Emoji": "🐅",
      "Meaning": "Tiger",
      "Unicode": "U+1F405"
    },
    {
      "Emoji": "🐆",
      "Meaning": "Leopard",
      "Unicode": "U+1F406"
    },
    {
      "Emoji": "🐴",
      "Meaning": "Horse face",
      "Unicode": "U+1F434"
    },
    {
      "Emoji": "🐎",
      "Meaning": "Horse",
      "Unicode": "U+1F40E"
    },
    {
      "Emoji": "🦄",
      "Meaning": "Unicorn",
      "Unicode": "U+1F984"
    },
    {
      "Emoji": "🦓",
      "Meaning": "Zebra",
      "Unicode": "U+1F993"
    },
    {
      "Emoji": "🦌",
      "Meaning": "Deer",
      "Unicode": "U+1F98C"
    },
    {
      "Emoji": "🦬",
      "Meaning": "Bison",
      "Unicode": "U+1F9AC"
    },
    {
      "Emoji": "🐮",
      "Meaning": "Cow face",
      "Unicode": "U+1F42E"
    },
    {
      "Emoji": "🐄",
      "Meaning": "Cow",
      "Unicode": "U+1F404"
    },
    {
      "Emoji": "🐂",
      "Meaning": "Ox",
      "Unicode": "U+1F402"
    },
    {
      "Emoji": "🐃",
      "Meaning": "Water buffalo",
      "Unicode": "U+1F437"
    },
    {
      "Emoji": "🐷",
      "Meaning": "Pig face",
      "Unicode": "U+1F437"
    },
    {
      "Emoji": "🐖",
      "Meaning": "Pig",
      "Unicode": "U+1F416"
    },
    {
      "Emoji": "🐗",
      "Meaning": "Boar",
      "Unicode": "U+1F417"
    },
    {
      "Emoji": "🐽",
      "Meaning": "Pig nose",
      "Unicode": "U+1F43D"
    },
    {
      "Emoji": "🐏",
      "Meaning": "Ram",
      "Unicode": "U+1F40F"
    },
    {
      "Emoji": "🐑",
      "Meaning": "Ewe",
      "Unicode": "U+1F411"
    },
    {
      "Emoji": "🐐",
      "Meaning": "Goat",
      "Unicode": "U+1F410"
    },
    {
      "Emoji": "🐪",
      "Meaning": "Camel",
      "Unicode": "U+1F42A"
    },
    {
      "Emoji": "🐫",
      "Meaning": "Two hump camel",
      "Unicode": "U+1F42B"
    },
    {
      "Emoji": "🦙",
      "Meaning": "Ilama",
      "Unicode": "U+1F999"
    },
    {
      "Emoji": "🦒",
      "Meaning": "Giraffe",
      "Unicode": "U+1F992"
    },
    {
      "Emoji": "🐘",
      "Meaning": "Elephant",
      "Unicode": "U+1F418"
    },
    {
      "Emoji": "🦣",
      "Meaning": "Mammoth",
      "Unicode": "U+1F9A3"
    },
    {
      "Emoji": "🦏",
      "Meaning": "Rhiniceros",
      "Unicode": "U+1F98F"
    },
    {
      "Emoji": "🦛",
      "Meaning": "Hippopotamus",
      "Unicode": "U+1F99B"
    },
    {
      "Emoji": "🐭",
      "Meaning": "Mouse face",
      "Unicode": "U+1F42D"
    },
    {
      "Emoji": "🐁",
      "Meaning": "Mouse",
      "Unicode": "U+1F401"
    },
    {
      "Emoji": "🐀",
      "Meaning": "Rat",
      "Unicode": "U+1F400"
    },
    {
      "Emoji": "🐹",
      "Meaning": "Hamster",
      "Unicode": "U+1F439"
    },
    {
      "Emoji": "🐰",
      "Meaning": "Rabbit face",
      "Unicode": "U+1F430"
    },
    {
      "Emoji": "🐇",
      "Meaning": "Rabbit",
      "Unicode": "U+1F407"
    },
    {
      "Emoji": "🐿",
      "Meaning": "Chipmunk",
      "Unicode": "U+1F43F"
    },
    {
      "Emoji": "🦫",
      "Meaning": "Beaver",
      "Unicode": "U+1F9AB"
    },
    {
      "Emoji": "🦔",
      "Meaning": "Hedgehog",
      "Unicode": "U+1F994"
    },
    {
      "Emoji": "🦇",
      "Meaning": "Bat",
      "Unicode": "U+1F987"
    },
    {
      "Emoji": "🐻",
      "Meaning": "Bear",
      "Unicode": "U+1F43B"
    },
    {
      "Emoji": "🐻‍❄️",
      "Meaning": "Polar bear",
      "Unicode": "U+200D"
    },
    {
      "Emoji": "🐨",
      "Meaning": "Koala",
      "Unicode": "U+1F428"
    },
    {
      "Emoji": "🐼",
      "Meaning": "Panda",
      "Unicode": "U+1F43C"
    },
    {
      "Emoji": "🦥",
      "Meaning": "Sloth",
      "Unicode": "U+1F9A5"
    },
    {
      "Emoji": "🦦",
      "Meaning": "Otter",
      "Unicode": "U+1F9A6"
    },
    {
      "Emoji": "🦨",
      "Meaning": "Skunk",
      "Unicode": "U+1F9A8"
    },
    {
      "Emoji": "🦘",
      "Meaning": "Kangaroo",
      "Unicode": "U+1F998"
    },
    {
      "Emoji": "🦡",
      "Meaning": "Badger",
      "Unicode": "U+1F9A1"
    },
    {
      "Emoji": "🐾",
      "Meaning": "Paw prints",
      "Unicode": "U+1F43E"
    },
    {
      "Emoji": "🦃",
      "Meaning": "Turkey",
      "Unicode": "U+1F983"
    },
    {
      "Emoji": "🐔",
      "Meaning": "Chicken",
      "Unicode": "U+1F414"
    },
    {
      "Emoji": "🐓",
      "Meaning": "Rooster",
      "Unicode": "U+1F413"
    },
    {
      "Emoji": "🐣",
      "Meaning": "Hatching",
      "Unicode": "U+1F423"
    },
    {
      "Emoji": "🐤",
      "Meaning": "Baby chick",
      "Unicode": "U+1F424"
    },
    {
      "Emoji": "🐥",
      "Meaning": "Front-facing chick",
      "Unicode": "U+1F425"
    },
    {
      "Emoji": "🐦",
      "Meaning": "Bird",
      "Unicode": "U+1F426"
    },
    {
      "Emoji": "🐧",
      "Meaning": "Penguin",
      "Unicode": "U+1F427"
    },
    {
      "Emoji": "🕊",
      "Meaning": "Dove",
      "Unicode": "U+1F54A"
    },
    {
      "Emoji": "🦅",
      "Meaning": "Eagle",
      "Unicode": "U+1F985"
    },
    {
      "Emoji": "🦆",
      "Meaning": "Duck",
      "Unicode": "U+1F986"
    },
    {
      "Emoji": "🦢",
      "Meaning": "Swan",
      "Unicode": "U+1F9A2"
    },
    {
      "Emoji": "🦉",
      "Meaning": "Owl",
      "Unicode": "U+1F989"
    },
    {
      "Emoji": "🦤",
      "Meaning": "Dodo",
      "Unicode": "U+1F9A4"
    },
    {
      "Emoji": "🪶",
      "Meaning": "Feather",
      "Unicode": "U+1FAB6"
    },
    {
      "Emoji": "🦩",
      "Meaning": "Flamingo",
      "Unicode": "U+1F9A9"
    },
    {
      "Emoji": "🦜",
      "Meaning": "Peacock",
      "Unicode": "U+1F99C"
    },
    {
      "Emoji": "🐸",
      "Meaning": "Frog",
      "Unicode": "U+1F438"
    },
    {
      "Emoji": "🐊",
      "Meaning": "Crocodile",
      "Unicode": "U+1F40A"
    },
    {
      "Emoji": "🐢",
      "Meaning": "Turtle",
      "Unicode": "U+1F422"
    },
    {
      "Emoji": "🦎",
      "Meaning": "Lizard",
      "Unicode": "U+1F98E"
    },
    {
      "Emoji": "🐍",
      "Meaning": "Snake",
      "Unicode": "U+1F40D"
    },
    {
      "Emoji": "🐲",
      "Meaning": "Dragon face",
      "Unicode": "U+1F432"
    },
    {
      "Emoji": "🐉",
      "Meaning": "Dragon",
      "Unicode": "U+1F409"
    },
    {
      "Emoji": "🦕",
      "Meaning": "Sauropod",
      "Unicode": "U+1F995"
    },
    {
      "Emoji": "🦖",
      "Meaning": "Tyranosaurus",
      "Unicode": "U+1F996"
    },
    {
      "Emoji": "🐳",
      "Meaning": "Spouting whale",
      "Unicode": "U+1F433"
    },
    {
      "Emoji": "🐋",
      "Meaning": "Whale",
      "Unicode": "U+1F40B"
    },
    {
      "Emoji": "🐬",
      "Meaning": "Dolphin",
      "Unicode": "U+1F42C"
    },
    {
      "Emoji": "🦭",
      "Meaning": "Seal",
      "Unicode": "U+1F9AD"
    },
    {
      "Emoji": "🐟",
      "Meaning": "Fish",
      "Unicode": "U+1F41F"
    },
    {
      "Emoji": "🐠",
      "Meaning": "Tropical fish",
      "Unicode": "U+1F420"
    },
    {
      "Emoji": "🐡",
      "Meaning": "Blowfish",
      "Unicode": "U+1F421"
    },
    {
      "Emoji": "🦈",
      "Meaning": "Shark",
      "Unicode": "U+1F988"
    },
    {
      "Emoji": "🐙",
      "Meaning": "Octopus",
      "Unicode": "U+1F419"
    },
    {
      "Emoji": "🐚",
      "Meaning": "Spiral shell",
      "Unicode": "U+1F41A"
    },
    {
      "Emoji": "🐌",
      "Meaning": "Snail",
      "Unicode": "U+1F40C"
    },
    {
      "Emoji": "🦋",
      "Meaning": "Butterfly",
      "Unicode": "U+1F98B"
    },
    {
      "Emoji": "🐛",
      "Meaning": "Bug",
      "Unicode": "U+1F41B"
    },
    {
      "Emoji": "🐜",
      "Meaning": "Ant",
      "Unicode": "U+1F41C"
    },
    {
      "Emoji": "🐝",
      "Meaning": "Honeybee",
      "Unicode": "U+1F41D"
    },
    {
      "Emoji": "🪲",
      "Meaning": "Beetle",
      "Unicode": "U+1FAB2"
    },
    {
      "Emoji": "🐞",
      "Meaning": "Lady Beetle",
      "Unicode": "U+1F41E"
    },
    {
      "Emoji": "🦗",
      "Meaning": "Cricket",
      "Unicode": "U+1F997"
    },
    {
      "Emoji": "🪳",
      "Meaning": "Cockroach",
      "Unicode": "U+1FAB3"
    },
    {
      "Emoji": "🕷",
      "Meaning": "Spider",
      "Unicode": "U+1F577"
    },
    {
      "Emoji": "🕸",
      "Meaning": "Spider web",
      "Unicode": "U+1F578"
    },
    {
      "Emoji": "🦂",
      "Meaning": "Scorpion",
      "Unicode": "U+1F982"
    },
    {
      "Emoji": "🦟",
      "Meaning": "Mosquito",
      "Unicode": "U+1F99F"
    },
    {
      "Emoji": "🪰",
      "Meaning": "Fly",
      "Unicode": "U+1FAB0"
    },
    {
      "Emoji": "🪱",
      "Meaning": "Worm",
      "Unicode": "U+1FAB1"
    },
    {
      "Emoji": "🦠",
      "Meaning": "Microbe",
      "Unicode": "U+1F9A0"
    },
    {
      "Emoji": "💐",
      "Meaning": "Bouquet",
      "Unicode": "U+1F490"
    },
    {
      "Emoji": "🌸",
      "Meaning": "Cherry blossom",
      "Unicode": "U+1F338"
    },
    {
      "Emoji": "💮",
      "Meaning": "White flower",
      "Unicode": "U+1F4AE"
    },
    {
      "Emoji": "🏵",
      "Meaning": "Rosette",
      "Unicode": "U+1F3F5"
    },
    {
      "Emoji": "🌹",
      "Meaning": "Rose",
      "Unicode": "U+1F339"
    },
    {
      "Emoji": "🥀",
      "Meaning": "Wilted flower",
      "Unicode": "U+1F940"
    },
    {
      "Emoji": "🌺",
      "Meaning": "Hibiscus",
      "Unicode": "U+1F33A"
    },
    {
      "Emoji": "🌻",
      "Meaning": "Sunflower",
      "Unicode": "U+1F33B"
    },
    {
      "Emoji": "🌼",
      "Meaning": "Blossom",
      "Unicode": "U+1F33C"
    },
    {
      "Emoji": "🌷",
      "Meaning": "Tulip",
      "Unicode": "U+1F337"
    },
    {
      "Emoji": "🌱",
      "Meaning": "Seedling",
      "Unicode": "U+1F331"
    },
    {
      "Emoji": "🪴",
      "Meaning": "Potted plant",
      "Unicode": "U+1FAB4"
    },
    {
      "Emoji": "🌲",
      "Meaning": "Evergreen tree",
      "Unicode": "U+1F332"
    },
    {
      "Emoji": "🌳",
      "Meaning": "Deciduous plant",
      "Unicode": "U+1F333"
    },
    {
      "Emoji": "🌴",
      "Meaning": "Palm tree",
      "Unicode": "U+1F334"
    },
    {
      "Emoji": "🌵",
      "Meaning": "Cactus",
      "Unicode": "U+1F335"
    },
    {
      "Emoji": "🌾",
      "Meaning": "Sheaf of rice",
      "Unicode": "U+1F33E"
    },
    {
      "Emoji": "🌿",
      "Meaning": "Herb",
      "Unicode": "U+1F33F"
    },
    {
      "Emoji": "☘",
      "Meaning": "Shamrock",
      "Unicode": "U+2618"
    },
    {
      "Emoji": "🍀",
      "Meaning": "Four leaf clover",
      "Unicode": "U+1F340"
    },
    {
      "Emoji": "🍁",
      "Meaning": "Maple leaf",
      "Unicode": "U+1F341"
    },
    {
      "Emoji": "🍂",
      "Meaning": "Fallen leaf",
      "Unicode": "U+1F342"
    },
    {
      "Emoji": "🍃",
      "Meaning": "Leaf fluttering in wind",
      "Unicode": "U+1F343"
    },
    {
      "Emoji": "🪴",
      "Meaning": "Empty nest",
      "Unicode": "U+1FAB9"
    },
    {
      "Emoji": "🪴",
      "Meaning": "Nest with eggs",
      "Unicode": "U+1FABA"
    },
    {
      "Emoji": "🍇",
      "Meaning": "Grapes",
      "Unicode": "U+1F347"
    },
    {
      "Emoji": "🍈",
      "Meaning": "Melon",
      "Unicode": "U+1F348"
    },
    {
      "Emoji": "🍉",
      "Meaning": "Water melon",
      "Unicode": "U+1F349"
    },
    {
      "Emoji": "🍊",
      "Meaning": "Tangerine",
      "Unicode": "U+1F34A"
    },
    {
      "Emoji": "🍋",
      "Meaning": "Lime",
      "Unicode": "U+1F34B"
    },
    {
      "Emoji": "🍌",
      "Meaning": "Banana",
      "Unicode": "U+1F34C"
    },
    {
      "Emoji": "🍍",
      "Meaning": "Pineapple",
      "Unicode": "U+1F34D"
    },
    {
      "Emoji": "🥭",
      "Meaning": "Mango",
      "Unicode": "U+1F96D"
    },
    {
      "Emoji": "🍎",
      "Meaning": "Red apple",
      "Unicode": "U+1F34E"
    },
    {
      "Emoji": "🍏",
      "Meaning": "Green apple",
      "Unicode": "U+1F34F"
    },
    {
      "Emoji": "🍐",
      "Meaning": "Pear",
      "Unicode": "U+1F350"
    },
    {
      "Emoji": "🍑",
      "Meaning": "Peach",
      "Unicode": "U+1F351"
    },
    {
      "Emoji": "🍒",
      "Meaning": "Cherries",
      "Unicode": "U+1F352"
    },
    {
      "Emoji": "🍓",
      "Meaning": "Strawberries",
      "Unicode": "U+1F353"
    },
    {
      "Emoji": "🫐",
      "Meaning": "Blueberries",
      "Unicode": "U+1FAD0"
    },
    {
      "Emoji": "🥝",
      "Meaning": "Kiwi fruit",
      "Unicode": "U+1F95D"
    },
    {
      "Emoji": "🍅",
      "Meaning": "Tomato",
      "Unicode": "U+1F345"
    },
    {
      "Emoji": "🫒",
      "Meaning": "Olive",
      "Unicode": "U+1FAD2"
    },
    {
      "Emoji": "🥥",
      "Meaning": "Coconut",
      "Unicode": "U+1F965"
    },
    {
      "Emoji": "🥑",
      "Meaning": "Avocado",
      "Unicode": "U+1F951"
    },
    {
      "Emoji": "🍆",
      "Meaning": "Eggplant",
      "Unicode": "U+1F346"
    },
    {
      "Emoji": "🥔",
      "Meaning": "Potato",
      "Unicode": "U+1F954"
    },
    {
      "Emoji": "🥕",
      "Meaning": "Carrot",
      "Unicode": "U+1F955"
    },
    {
      "Emoji": "🌽",
      "Meaning": "Corn",
      "Unicode": "U+1F33D"
    },
    {
      "Emoji": "🌶",
      "Meaning": "Pepper",
      "Unicode": "U+1F336"
    },
    {
      "Emoji": "🫑",
      "Meaning": "Bell pepper",
      "Unicode": "U+1FAD1"
    },
    {
      "Emoji": "🥒",
      "Meaning": "Cucumber",
      "Unicode": "U+1F952"
    },
    {
      "Emoji": "🥬",
      "Meaning": "Leafy green",
      "Unicode": "U+1F96C"
    },
    {
      "Emoji": "🥦",
      "Meaning": "Broccoli",
      "Unicode": "U+1F966"
    },
    {
      "Emoji": "🧄",
      "Meaning": "Garlic",
      "Unicode": "U+1F9C4"
    },
    {
      "Emoji": "🧅",
      "Meaning": "Onion",
      "Unicode": "U+1F9C5"
    },
    {
      "Emoji": "🍄",
      "Meaning": "Mushroom",
      "Unicode": "U+1F344"
    },
    {
      "Emoji": "🥜",
      "Meaning": "Peanuts",
      "Unicode": "U+1F95C"
    },
    {
      "Emoji": "🫑",
      "Meaning": "Beans",
      "Unicode": "U+1FAD8"
    },
    {
      "Emoji": "🌰",
      "Meaning": "Chestnut",
      "Unicode": "U+1F330"
    },
    {
      "Emoji": "🍞",
      "Meaning": "Bread",
      "Unicode": "U+1F35E"
    },
    {
      "Emoji": "🥐",
      "Meaning": "Croissant",
      "Unicode": "U+1F950"
    },
    {
      "Emoji": "🥖",
      "Meaning": "Baguette bread",
      "Unicode": "U+1F956"
    },
    {
      "Emoji": "🫓",
      "Meaning": "Flat bread",
      "Unicode": "U+1FAD3"
    },
    {
      "Emoji": "🥨",
      "Meaning": "Pretzel",
      "Unicode": "U+1F968"
    },
    {
      "Emoji": "🥯",
      "Meaning": "Bagel",
      "Unicode": "U+1F96F"
    },
    {
      "Emoji": "🥞",
      "Meaning": "Pancake",
      "Unicode": "U+1F95E"
    },
    {
      "Emoji": "🧇",
      "Meaning": "Waffle",
      "Unicode": "U+1F9C7"
    },
    {
      "Emoji": "🧀",
      "Meaning": "Cheese wedge",
      "Unicode": "U+1F9C0"
    },
    {
      "Emoji": "🍖",
      "Meaning": "Meat with bone",
      "Unicode": "U+1F356"
    },
    {
      "Emoji": "🍗",
      "Meaning": "Poultry leg",
      "Unicode": "U+1F357"
    },
    {
      "Emoji": "🥩",
      "Meaning": "Cut of meat",
      "Unicode": "U+1F969"
    },
    {
      "Emoji": "🥓",
      "Meaning": "Bacon",
      "Unicode": "U+1F953"
    },
    {
      "Emoji": "🍔",
      "Meaning": "Hamburger",
      "Unicode": "U+1F354"
    },
    {
      "Emoji": "🍟",
      "Meaning": "French fries",
      "Unicode": "U+1F35F"
    },
    {
      "Emoji": "🍕",
      "Meaning": "Pizza",
      "Unicode": "U+1F355"
    },
    {
      "Emoji": "🌭",
      "Meaning": "Hot dog",
      "Unicode": "U+1F32D"
    },
    {
      "Emoji": "🥪",
      "Meaning": "Sandwich",
      "Unicode": "U+1F96A"
    },
    {
      "Emoji": "🌮",
      "Meaning": "Taco",
      "Unicode": "U+1F32E"
    },
    {
      "Emoji": "🌯",
      "Meaning": "Burrito",
      "Unicode": "U+1F32F"
    },
    {
      "Emoji": "🫔",
      "Meaning": "Tamale",
      "Unicode": "U+1FAD4"
    },
    {
      "Emoji": "🥙",
      "Meaning": "Stuffed flatbread",
      "Unicode": "U+1F959"
    },
    {
      "Emoji": "🧆",
      "Meaning": "Falafel",
      "Unicode": "U+1F9C6"
    },
    {
      "Emoji": "🥚",
      "Meaning": "Egg",
      "Unicode": "U+1F95A"
    },
    {
      "Emoji": "🍳",
      "Meaning": "Cooking",
      "Unicode": "U+1F373"
    },
    {
      "Emoji": "🥘",
      "Meaning": "Shallow pan of food",
      "Unicode": "U+1F958"
    },
    {
      "Emoji": "🍲",
      "Meaning": "Pot of food",
      "Unicode": "U+1F372"
    },
    {
      "Emoji": "🫕",
      "Meaning": "Fondue",
      "Unicode": "U+1FAD5"
    },
    {
      "Emoji": "🥣",
      "Meaning": "Bowl with food",
      "Unicode": "U+1F963"
    },
    {
      "Emoji": "🥗",
      "Meaning": "Green salad",
      "Unicode": "U+1F957"
    },
    {
      "Emoji": "🍿",
      "Meaning": "Popcorn",
      "Unicode": "U+1F37F"
    },
    {
      "Emoji": "🧈",
      "Meaning": "Butter",
      "Unicode": "U+1F9C8"
    },
    {
      "Emoji": "🧂",
      "Meaning": "Salt",
      "Unicode": "U+1F9C2"
    },
    {
      "Emoji": "🥫",
      "Meaning": "Canned food",
      "Unicode": "U+1F96B"
    },
    {
      "Emoji": "🍱",
      "Meaning": "Bento box",
      "Unicode": "U+1F371"
    },
    {
      "Emoji": "🍘",
      "Meaning": "RIce cracker",
      "Unicode": "U+1F358"
    },
    {
      "Emoji": "🍙",
      "Meaning": "Rice ball",
      "Unicode": "U+1F359"
    },
    {
      "Emoji": "🍚",
      "Meaning": "Cooked rice",
      "Unicode": "U+1F35A"
    },
    {
      "Emoji": "🍛",
      "Meaning": "Curry rice",
      "Unicode": "U+1F35B"
    },
    {
      "Emoji": "🍜",
      "Meaning": "Steaming bowl",
      "Unicode": "U+1F35C"
    },
    {
      "Emoji": "🍝",
      "Meaning": "Spaghetti",
      "Unicode": "U+1F35D"
    },
    {
      "Emoji": "🍠",
      "Meaning": "Roasted sweet potato",
      "Unicode": "U+1F360"
    },
    {
      "Emoji": "🍢",
      "Meaning": "Oden",
      "Unicode": "U+1F362"
    },
    {
      "Emoji": "🍣",
      "Meaning": "Sushi",
      "Unicode": "U+1F363"
    },
    {
      "Emoji": "🍤",
      "Meaning": "Fried shrimp",
      "Unicode": "U+1F364"
    },
    {
      "Emoji": "🍥",
      "Meaning": "Fish cake with swiri",
      "Unicode": "U+1F365"
    },
    {
      "Emoji": "🥮",
      "Meaning": "Moon cake",
      "Unicode": "U+1F96E"
    },
    {
      "Emoji": "🍡",
      "Meaning": "Dango",
      "Unicode": "U+1F361"
    },
    {
      "Emoji": "🥟",
      "Meaning": "Dumpling",
      "Unicode": "U+1F95F"
    },
    {
      "Emoji": "🥠",
      "Meaning": "Fortune cookie",
      "Unicode": "U+1F960"
    },
    {
      "Emoji": "🥡",
      "Meaning": "Take out box",
      "Unicode": "U+1F961"
    },
    {
      "Emoji": "🦀",
      "Meaning": "Crab",
      "Unicode": "U+1F980"
    },
    {
      "Emoji": "🦞",
      "Meaning": "Lobster",
      "Unicode": "U+1F99E"
    },
    {
      "Emoji": "🦐",
      "Meaning": "Shrimp",
      "Unicode": "U+1F990"
    },
    {
      "Emoji": "🦑",
      "Meaning": "Squid",
      "Unicode": "U+1F991"
    },
    {
      "Emoji": "🦪",
      "Meaning": "Oyster",
      "Unicode": "U+1F9AA"
    },
    {
      "Emoji": "🍨",
      "Meaning": "Ice cream",
      "Unicode": "U+1F368"
    },
    {
      "Emoji": "🍧",
      "Meaning": "Shaved ice cream",
      "Unicode": "U+1F367"
    },
    {
      "Emoji": "🍦",
      "Meaning": "Soft ice cream",
      "Unicode": "U+1F366"
    },
    {
      "Emoji": "🍩",
      "Meaning": "Doughnut",
      "Unicode": "U+1F369"
    },
    {
      "Emoji": "🍪",
      "Meaning": "Cookie",
      "Unicode": "U+1F36A"
    },
    {
      "Emoji": "🎂",
      "Meaning": "Birthday cake",
      "Unicode": "U+1F382"
    },
    {
      "Emoji": "🍰",
      "Meaning": "Short cake",
      "Unicode": "U+1F370"
    },
    {
      "Emoji": "🧁",
      "Meaning": "Cup cake",
      "Unicode": "U+1F9C1"
    },
    {
      "Emoji": "🥧",
      "Meaning": "Pie",
      "Unicode": "U+1F967"
    },
    {
      "Emoji": "🍫",
      "Meaning": "Chocoloate",
      "Unicode": "U+1F36B"
    },
    {
      "Emoji": "🍬",
      "Meaning": "Candy",
      "Unicode": "U+1F36C"
    },
    {
      "Emoji": "🍭",
      "Meaning": "Lollipop",
      "Unicode": "U+1F36D"
    },
    {
      "Emoji": "🍮",
      "Meaning": "Custard",
      "Unicode": "U+1F36E"
    },
    {
      "Emoji": "🍯",
      "Meaning": "Honey pot",
      "Unicode": "U+1F36F"
    },
    {
      "Emoji": "🍼",
      "Meaning": "Baby bottle",
      "Unicode": "U+1F37C"
    },
    {
      "Emoji": "🥛",
      "Meaning": "Glass of milk",
      "Unicode": "U+1F95B"
    },
    {
      "Emoji": "☕",
      "Meaning": "Hot beverage",
      "Unicode": "U+2615"
    },
    {
      "Emoji": "🫖",
      "Meaning": "Teapot",
      "Unicode": "U+1FAD6"
    },
    {
      "Emoji": "🍵",
      "Meaning": "Teacup without handle",
      "Unicode": "U+1F375"
    },
    {
      "Emoji": "🍶",
      "Meaning": "Sake",
      "Unicode": "U+1F376"
    },
    {
      "Emoji": "🍾",
      "Meaning": "Bottle with poppin cork",
      "Unicode": "U+1F37E"
    },
    {
      "Emoji": "🍷",
      "Meaning": "Wine glass",
      "Unicode": "U+1F377"
    },
    {
      "Emoji": "🍸",
      "Meaning": "Cocktail glass",
      "Unicode": "U+1F378"
    },
    {
      "Emoji": "🍹",
      "Meaning": "Tropical drink",
      "Unicode": "U+1F379"
    },
    {
      "Emoji": "🍺",
      "Meaning": "Beer mug",
      "Unicode": "U+1F37A"
    },
    {
      "Emoji": "🍻",
      "Meaning": "Clinking beer mug",
      "Unicode": "U+1F37B"
    },
    {
      "Emoji": "🥂",
      "Meaning": "Clinking glasses",
      "Unicode": "U+1F942"
    },
    {
      "Emoji": "🥃",
      "Meaning": "Tumbler glass",
      "Unicode": "U+1F943"
    },
    {
      "Emoji": "🥤",
      "Meaning": "Cup with strawberry",
      "Unicode": "U+1F964"
    },
    {
      "Emoji": "🧋",
      "Meaning": "Bubble tea",
      "Unicode": "U+1F9CB"
    },
    {
      "Emoji": "🧃",
      "Meaning": "Beverage box",
      "Unicode": "U+1F9C3"
    },
    {
      "Emoji": "🧉",
      "Meaning": "Mate",
      "Unicode": "U+1F9C9"
    },
    {
      "Emoji": "🧊",
      "Meaning": "Ice",
      "Unicode": "U+1F9CA"
    },
    {
      "Emoji": "🥢",
      "Meaning": "Chopsticks",
      "Unicode": "U+1F962"
    },
    {
      "Emoji": "🍽",
      "Meaning": "Fork and knife with plate",
      "Unicode": "U+1F37D"
    },
    {
      "Emoji": "🍴",
      "Meaning": "Fork and knife",
      "Unicode": "U+1F374"
    },
    {
      "Emoji": "🥄",
      "Meaning": "Spoon",
      "Unicode": "U+1F944"
    },
    {
      "Emoji": "🔪",
      "Meaning": "Kitchen knife",
      "Unicode": "U+1F52A"
    },
    {
      "Emoji": "🧋",
      "Meaning": "Jar",
      "Unicode": "U+1FAD9"
    },
    {
      "Emoji": "🏺",
      "Meaning": "Amphora",
      "Unicode": "U+1F3FA"
    },
    {
      "Emoji": "🌍",
      "Meaning": "Globe showing Africa and Europe",
      "Unicode": "U+1F30D"
    },
    {
      "Emoji": "🌎",
      "Meaning": "Globe showing Americas",
      "Unicode": "U+1F30E"
    },
    {
      "Emoji": "🌏",
      "Meaning": "Globe showing Asia and Australia",
      "Unicode": "U+1F30F"
    },
    {
      "Emoji": "🌐",
      "Meaning": "Globe with meridians",
      "Unicode": "U+1F310"
    },
    {
      "Emoji": "🗺",
      "Meaning": "World map",
      "Unicode": "U+1F5FA"
    },
    {
      "Emoji": "🧭",
      "Meaning": "Compass",
      "Unicode": "U+1F9ED"
    },
    {
      "Emoji": "⛰",
      "Meaning": "Mountain",
      "Unicode": "U+26F0"
    },
    {
      "Emoji": "🏔",
      "Meaning": "Snowcap mountain",
      "Unicode": "U+26F0"
    },
    {
      "Emoji": "🌋",
      "Meaning": "Volcanic mountain",
      "Unicode": "U+1F30B"
    },
    {
      "Emoji": "🗻",
      "Meaning": "Fuji mountain",
      "Unicode": "U+1F5FB"
    },
    {
      "Emoji": "🏕",
      "Meaning": "Camping",
      "Unicode": "U+1F3D5"
    },
    {
      "Emoji": "🏖",
      "Meaning": "Beach with umbrella",
      "Unicode": "U+1F3D6"
    },
    {
      "Emoji": "🏜",
      "Meaning": "Desert",
      "Unicode": "U+1F3DC"
    },
    {
      "Emoji": "🏝",
      "Meaning": "Desertified island",
      "Unicode": "U+1F3DD"
    },
    {
      "Emoji": "🏞",
      "Meaning": "National park",
      "Unicode": "U+1F3DE"
    },
    {
      "Emoji": "🏟",
      "Meaning": "Stadium",
      "Unicode": "U+1F3DF"
    },
    {
      "Emoji": "🏛",
      "Meaning": "Classical building",
      "Unicode": "U+1F3DB"
    },
    {
      "Emoji": "🏗",
      "Meaning": "Building construction",
      "Unicode": "U+1F3D7"
    },
    {
      "Emoji": "🧱",
      "Meaning": "Brick",
      "Unicode": "U+1F9F1"
    },
    {
      "Emoji": "🪨",
      "Meaning": "Rock",
      "Unicode": "U+1FAA8"
    },
    {
      "Emoji": "🪵",
      "Meaning": "Wood",
      "Unicode": "U+1FAB5"
    },
    {
      "Emoji": "🛖",
      "Meaning": "Hut",
      "Unicode": "U+1F6D6"
    },
    {
      "Emoji": "🏘",
      "Meaning": "Houses",
      "Unicode": "U+1F3D8"
    },
    {
      "Emoji": "🏚",
      "Meaning": "Derelict house",
      "Unicode": "U+1F3DA"
    },
    {
      "Emoji": "🏠",
      "Meaning": "House",
      "Unicode": "U+1F3E0"
    },
    {
      "Emoji": "🏡",
      "Meaning": "House with garden",
      "Unicode": "U+1F3E1"
    },
    {
      "Emoji": "🏢",
      "Meaning": "Office building",
      "Unicode": "U+1F3E2"
    },
    {
      "Emoji": "🏣",
      "Meaning": "Japanese office",
      "Unicode": "U+1F3E3"
    },
    {
      "Emoji": "🏤",
      "Meaning": "Post office",
      "Unicode": "U+1F3E4"
    },
    {
      "Emoji": "🏥",
      "Meaning": "Hospital",
      "Unicode": "U+1F3E5"
    },
    {
      "Emoji": "🏦",
      "Meaning": "Bank",
      "Unicode": "U+1F3E6"
    },
    {
      "Emoji": "🏨",
      "Meaning": "Hotel",
      "Unicode": "U+1F3E8"
    },
    {
      "Emoji": "🏩",
      "Meaning": "Love hotel",
      "Unicode": "U+1F3E9"
    },
    {
      "Emoji": "🏪",
      "Meaning": "Convenience store",
      "Unicode": "U+1F3EA"
    },
    {
      "Emoji": "🏫",
      "Meaning": "School",
      "Unicode": "U+1F3EB"
    },
    {
      "Emoji": "🏬",
      "Meaning": "Department",
      "Unicode": "U+1F3EC"
    },
    {
      "Emoji": "🏭",
      "Meaning": "Factory",
      "Unicode": "U+1F3ED"
    },
    {
      "Emoji": "🏯",
      "Meaning": "Japanese castle",
      "Unicode": "U+1F3EF"
    },
    {
      "Emoji": "🏰",
      "Meaning": "Castle",
      "Unicode": "U+1F3F0"
    },
    {
      "Emoji": "💒",
      "Meaning": "Wedding house",
      "Unicode": "U+1F492"
    },
    {
      "Emoji": "🗼",
      "Meaning": "Tokyo tower",
      "Unicode": "U+1F5FC"
    },
    {
      "Emoji": "🗽",
      "Meaning": "Statue of liberty",
      "Unicode": "U+1F5FD"
    },
    {
      "Emoji": "⛪",
      "Meaning": "Church",
      "Unicode": "U+26EA"
    },
    {
      "Emoji": "🕌",
      "Meaning": "Mosque",
      "Unicode": "U+1F54C"
    },
    {
      "Emoji": "🛕",
      "Meaning": "Hindu temple",
      "Unicode": "U+1F6D5"
    },
    {
      "Emoji": "🕍",
      "Meaning": "Synagogue",
      "Unicode": "U+1F54D"
    },
    {
      "Emoji": "⛩",
      "Meaning": "Shinto shrine",
      "Unicode": "U+26E9"
    },
    {
      "Emoji": "🕋",
      "Meaning": "Kaaba",
      "Unicode": "U+1F54B"
    },
    {
      "Emoji": "⛲",
      "Meaning": "Fountain",
      "Unicode": "U+26F2"
    },
    {
      "Emoji": "⛺",
      "Meaning": "Tent",
      "Unicode": "U+26FA"
    },
    {
      "Emoji": "🌁",
      "Meaning": "Foggy",
      "Unicode": "U+1F301"
    },
    {
      "Emoji": "🌃",
      "Meaning": "Night with starrs",
      "Unicode": "U+1F303"
    },
    {
      "Emoji": "🏙",
      "Meaning": "Citscape",
      "Unicode": "U+1F3D9"
    },
    {
      "Emoji": "🌅",
      "Meaning": "Sunrise",
      "Unicode": "U+1F305"
    },
    {
      "Emoji": "🌄",
      "Meaning": "Sunrise over mountains",
      "Unicode": "U+1F304"
    },
    {
      "Emoji": "🌆",
      "Meaning": "Cityscape at dusk",
      "Unicode": "U+1F306"
    },
    {
      "Emoji": "🌇",
      "Meaning": "Sunset",
      "Unicode": "U+1F307"
    },
    {
      "Emoji": "🌉",
      "Meaning": "Bridge at night",
      "Unicode": "U+1F309"
    },
    {
      "Emoji": "♨",
      "Meaning": "Hot springs",
      "Unicode": "U+2668"
    },
    {
      "Emoji": "🎠",
      "Meaning": "Carousel horse",
      "Unicode": "U+1F3A0"
    },
    {
      "Emoji": "🎡",
      "Meaning": "Ferris wheel",
      "Unicode": "U+1F3A1"
    },
    {
      "Emoji": "🎢",
      "Meaning": "Roller coaster",
      "Unicode": "U+1F3A2"
    },
    {
      "Emoji": "💈",
      "Meaning": "Barber poll",
      "Unicode": "U+1F488"
    },
    {
      "Emoji": "🎪",
      "Meaning": "Circus tent",
      "Unicode": "U+1F3AA"
    },
    {
      "Emoji": "🚂",
      "Meaning": "Locomotive",
      "Unicode": "U+1F682"
    },
    {
      "Emoji": "🚃",
      "Meaning": "Railway car",
      "Unicode": "U+1F683"
    },
    {
      "Emoji": "🚄",
      "Meaning": "High speed train",
      "Unicode": "U+1F684"
    },
    {
      "Emoji": "🚅",
      "Meaning": "Bullet train",
      "Unicode": "U+1F685"
    },
    {
      "Emoji": "🚆",
      "Meaning": "Train",
      "Unicode": "U+1F686"
    },
    {
      "Emoji": "🚇",
      "Meaning": "Metro",
      "Unicode": "U+1F687"
    },
    {
      "Emoji": "🚈",
      "Meaning": "Light rail",
      "Unicode": "U+1F688"
    },
    {
      "Emoji": "🚉",
      "Meaning": "Station",
      "Unicode": "U+1F689"
    },
    {
      "Emoji": "🚊",
      "Meaning": "Tram",
      "Unicode": "U+1F68A"
    },
    {
      "Emoji": "🚝",
      "Meaning": "Monorail",
      "Unicode": "U+1F69D"
    },
    {
      "Emoji": "🚞",
      "Meaning": "Mountain railway",
      "Unicode": "U+1F69E"
    },
    {
      "Emoji": "🚋",
      "Meaning": "Tram car",
      "Unicode": "U+1F68B"
    },
    {
      "Emoji": "🚌",
      "Meaning": "us",
      "Unicode": "U+1F68C"
    },
    {
      "Emoji": "🚍",
      "Meaning": "Oncoming bus",
      "Unicode": "U+1F68D"
    },
    {
      "Emoji": "🚎",
      "Meaning": "Trolley bus",
      "Unicode": "U+1F68E"
    },
    {
      "Emoji": "🚐",
      "Meaning": "Minibus",
      "Unicode": "U+1F690"
    },
    {
      "Emoji": "🚑",
      "Meaning": "Ambulance",
      "Unicode": "U+1F691"
    },
    {
      "Emoji": "🚒",
      "Meaning": "Fire engine",
      "Unicode": "U+1F692"
    },
    {
      "Emoji": "🚓",
      "Meaning": "Police car",
      "Unicode": "U+1F693"
    },
    {
      "Emoji": "🚔",
      "Meaning": "Oncoming police car",
      "Unicode": "U+1F694"
    },
    {
      "Emoji": "🚕",
      "Meaning": "Taxi",
      "Unicode": "U+1F695"
    },
    {
      "Emoji": "🚖",
      "Meaning": "Oncoming taxi",
      "Unicode": "U+1F696"
    },
    {
      "Emoji": "🚗",
      "Meaning": "Automobile",
      "Unicode": "U+1F697"
    },
    {
      "Emoji": "🚘",
      "Meaning": "Oncoming automobile",
      "Unicode": "U+1F698"
    },
    {
      "Emoji": "🚙",
      "Meaning": "Sport utility vehicle",
      "Unicode": "U+1F699"
    },
    {
      "Emoji": "🛻",
      "Meaning": "Pickup truck",
      "Unicode": "U+1F6FB"
    },
    {
      "Emoji": "🚚",
      "Meaning": "Delivery truck",
      "Unicode": "U+1F69A"
    },
    {
      "Emoji": "🚛",
      "Meaning": "Articulated lorry",
      "Unicode": "U+1F69B"
    },
    {
      "Emoji": "🚜",
      "Meaning": "Tractor",
      "Unicode": "U+1F69C"
    },
    {
      "Emoji": "🏎",
      "Meaning": "Racing car",
      "Unicode": "U+1F3CE"
    },
    {
      "Emoji": "🏍",
      "Meaning": "Motorcycle",
      "Unicode": "U+1F3CD"
    },
    {
      "Emoji": "🛵",
      "Meaning": "Scooter",
      "Unicode": "U+1F6F5"
    },
    {
      "Emoji": "🦽",
      "Meaning": "Manual wheelchair",
      "Unicode": "U+1F9BD"
    },
    {
      "Emoji": "🦼",
      "Meaning": "Motorized wheelchair",
      "Unicode": "U+1F9BC"
    },
    {
      "Emoji": "⌛",
      "Meaning": "Hourglass done",
      "Unicode": "U+231B"
    },
    {
      "Emoji": "⏳",
      "Meaning": "Hourglass starting",
      "Unicode": "U+23F3"
    },
    {
      "Emoji": "⌚",
      "Meaning": "Watch",
      "Unicode": "U+231A"
    },
    {
      "Emoji": "⏰",
      "Meaning": "Alarm",
      "Unicode": "U+23F0"
    },
    {
      "Emoji": "⏱",
      "Meaning": "Stopwatch",
      "Unicode": "U+23F1"
    },
    {
      "Emoji": "⏲",
      "Meaning": "Timer clock",
      "Unicode": "U+23F2"
    },
    {
      "Emoji": "🕰",
      "Meaning": "Mantelpiece clock",
      "Unicode": "U+1F570"
    },
    {
      "Emoji": "🕛",
      "Meaning": "Twelve O'clock",
      "Unicode": "U+1F55B"
    },
    {
      "Emoji": "🕧",
      "Meaning": "Twelve-thirty",
      "Unicode": "U+1F567"
    },
    {
      "Emoji": "🕐",
      "Meaning": "One O'clock",
      "Unicode": "U+1F550"
    },
    {
      "Emoji": "🕜",
      "Meaning": "One-thirty",
      "Unicode": "U+1F55C"
    },
    {
      "Emoji": "🕑",
      "Meaning": "Two O'clock",
      "Unicode": "U+1F551"
    },
    {
      "Emoji": "🕝",
      "Meaning": "Two-thirty",
      "Unicode": "U+1F55D"
    },
    {
      "Emoji": "🕒",
      "Meaning": "Three O'clock",
      "Unicode": "U+1F552"
    },
    {
      "Emoji": "🕞",
      "Meaning": "Three-thirty",
      "Unicode": "U+1F55E"
    },
    {
      "Emoji": "🕓",
      "Meaning": "Four O'clock",
      "Unicode": "U+1F553"
    },
    {
      "Emoji": "🕟",
      "Meaning": "Four-thirty",
      "Unicode": "U+1F55F"
    },
    {
      "Emoji": "🕔",
      "Meaning": "Five O'clock",
      "Unicode": "U+1F554"
    },
    {
      "Emoji": "🕠",
      "Meaning": "Five-thirty",
      "Unicode": "U+1F560"
    },
    {
      "Emoji": "🕕",
      "Meaning": "Six O'clock",
      "Unicode": "U+1F555"
    },
    {
      "Emoji": "🕡",
      "Meaning": "Six-thirty",
      "Unicode": "U+1F561"
    },
    {
      "Emoji": "🕖",
      "Meaning": "Seven O'clock",
      "Unicode": "U+1F556"
    },
    {
      "Emoji": "🕢",
      "Meaning": "Seven-thirty",
      "Unicode": "U+1F562"
    },
    {
      "Emoji": "🕗",
      "Meaning": "Eight O'clock",
      "Unicode": "U+1F557"
    },
    {
      "Emoji": "🕣",
      "Meaning": "Eight-thirty",
      "Unicode": "U+1F563"
    },
    {
      "Emoji": "🕘",
      "Meaning": "Nine O'clock",
      "Unicode": "U+1F558"
    },
    {
      "Emoji": "🕤",
      "Meaning": "Nine-thirty",
      "Unicode": "U+1F564"
    },
    {
      "Emoji": "🕙",
      "Meaning": "Ten O'clock",
      "Unicode": "U+1F559"
    },
    {
      "Emoji": "🕥",
      "Meaning": "Ten-thirty",
      "Unicode": "U+1F565"
    },
    {
      "Emoji": "🕚",
      "Meaning": "Eleven O'clock",
      "Unicode": "U+1F55A"
    },
    {
      "Emoji": "🕦",
      "Meaning": "Eleven-thirty",
      "Unicode": "U+1F566"
    },
    {
      "Emoji": "🌑",
      "Meaning": "New moon",
      "Unicode": "U+1F311"
    },
    {
      "Emoji": "🌒",
      "Meaning": "Waxing crescent moon",
      "Unicode": "U+1F312"
    },
    {
      "Emoji": "🌓",
      "Meaning": "First quarter moon",
      "Unicode": "U+1F313"
    },
    {
      "Emoji": "🌔",
      "Meaning": "Waxing gibbous moon",
      "Unicode": "U+1F314"
    },
    {
      "Emoji": "🌕",
      "Meaning": "Full moon",
      "Unicode": "U+1F315"
    },
    {
      "Emoji": "🌖",
      "Meaning": "Waning gibbous moon",
      "Unicode": "U+1F316"
    },
    {
      "Emoji": "🌗",
      "Meaning": "Last quarter moon",
      "Unicode": "U+1F317"
    },
    {
      "Emoji": "🌘",
      "Meaning": "Waning crescent moon",
      "Unicode": "U+1F318"
    },
    {
      "Emoji": "🌙",
      "Meaning": "Crescent moon",
      "Unicode": "U+1F319"
    },
    {
      "Emoji": "🌚",
      "Meaning": "New moon face",
      "Unicode": "U+1F31A"
    },
    {
      "Emoji": "🌛",
      "Meaning": "First quarter moon face",
      "Unicode": "U+1F31B"
    },
    {
      "Emoji": "🌜",
      "Meaning": "Last quartermoon face",
      "Unicode": "U+1F31C"
    },
    {
      "Emoji": "🌡",
      "Meaning": "Thermometer",
      "Unicode": "U+1F321"
    },
    {
      "Emoji": "☀",
      "Meaning": "Sun",
      "Unicode": "U+2600"
    },
    {
      "Emoji": "🌝",
      "Meaning": "Full moon face",
      "Unicode": "U+1F31D"
    },
    {
      "Emoji": "🌞",
      "Meaning": "Sun with face",
      "Unicode": "U+1F31E"
    },
    {
      "Emoji": "🪐",
      "Meaning": "Ringed planet",
      "Unicode": "U+1FA90"
    },
    {
      "Emoji": "⭐",
      "Meaning": "Star",
      "Unicode": "U+2B50"
    },
    {
      "Emoji": "🌟",
      "Meaning": "Glowing star",
      "Unicode": "U+1F31F"
    },
    {
      "Emoji": "🌠",
      "Meaning": "Shooting star",
      "Unicode": "U+1F320"
    },
    {
      "Emoji": "🌌",
      "Meaning": "Milky way",
      "Unicode": "U+1F30C"
    },
    {
      "Emoji": "☁",
      "Meaning": "Cloud",
      "Unicode": "U+2601"
    },
    {
      "Emoji": "⛅",
      "Meaning": "Sun behind cloud",
      "Unicode": "U+26C5"
    },
    {
      "Emoji": "⛈",
      "Meaning": "Cloud with lighting and rain",
      "Unicode": "U+26C8"
    },
    {
      "Emoji": "🌤",
      "Meaning": "Sun behind small cloud",
      "Unicode": "U+1F324"
    },
    {
      "Emoji": "🌥",
      "Meaning": "Sun behind large cloud",
      "Unicode": "U+1F325"
    },
    {
      "Emoji": "🌦",
      "Meaning": "Sun behind rain cloud",
      "Unicode": "U+1F326"
    },
    {
      "Emoji": "🌧",
      "Meaning": "Cloud with rain",
      "Unicode": "U+1F327"
    },
    {
      "Emoji": "🌨",
      "Meaning": "Cloud with snow",
      "Unicode": "U+1F328"
    },
    {
      "Emoji": "🌩",
      "Meaning": "Cloud with lighting",
      "Unicode": "U+1005"
    },
    {
      "Emoji": "🌪",
      "Meaning": "Tornado",
      "Unicode": "U+1F32A"
    },
    {
      "Emoji": "🌫",
      "Meaning": "Fog",
      "Unicode": "U+1F32B"
    },
    {
      "Emoji": "🌬",
      "Meaning": "Wind face",
      "Unicode": "U+1F32C"
    },
    {
      "Emoji": "🌀",
      "Meaning": "Cyclone",
      "Unicode": "U+1F300"
    },
    {
      "Emoji": "🌈",
      "Meaning": "Rainbow",
      "Unicode": "U+1F308"
    },
    {
      "Emoji": "🌂",
      "Meaning": "Closed umbrella",
      "Unicode": "U+1F302"
    },
    {
      "Emoji": "☂",
      "Meaning": "Umbrella",
      "Unicode": "U+2602"
    },
    {
      "Emoji": "☔",
      "Meaning": "Umbrella with raindrops",
      "Unicode": "U+2614"
    },
    {
      "Emoji": "⛱",
      "Meaning": "Umbrella on ground",
      "Unicode": "U+26F1"
    },
    {
      "Emoji": "⚡",
      "Meaning": "High voltage",
      "Unicode": "U+26A1"
    },
    {
      "Emoji": "❄",
      "Meaning": "Snowflake",
      "Unicode": "U+2744"
    },
    {
      "Emoji": "☃",
      "Meaning": "Snowman",
      "Unicode": "U+2603"
    },
    {
      "Emoji": "⛄",
      "Meaning": "Snowman without snow",
      "Unicode": "U+26C4"
    },
    {
      "Emoji": "☄",
      "Meaning": "Comet",
      "Unicode": "U+2604"
    },
    {
      "Emoji": "🔥",
      "Meaning": "Fire",
      "Unicode": "U+1F525"
    },
    {
      "Emoji": "💧",
      "Meaning": "Droplet",
      "Unicode": "U+1F4A7"
    },
    {
      "Emoji": "🌊",
      "Meaning": "Water wave",
      "Unicode": "U+1F30A"
    },
    {
      "Emoji": "🎃",
      "Meaning": "Jack-o-lantern",
      "Unicode": "U+1F383"
    },
    {
      "Emoji": "🎄",
      "Meaning": "Christmas tree",
      "Unicode": "U+1F384"
    },
    {
      "Emoji": "🎆",
      "Meaning": "Fireworks",
      "Unicode": "U+1F386"
    },
    {
      "Emoji": "🎇",
      "Meaning": "Sparkler",
      "Unicode": "U+1F387"
    },
    {
      "Emoji": "🧨",
      "Meaning": "Firecracker",
      "Unicode": "U+1F9E8"
    },
    {
      "Emoji": "✨",
      "Meaning": "Sparkles",
      "Unicode": "U+2728"
    },
    {
      "Emoji": "🎈",
      "Meaning": "Baloon",
      "Unicode": "U+1F388"
    },
    {
      "Emoji": "🎉",
      "Meaning": "Party popper",
      "Unicode": "U+1F389"
    },
    {
      "Emoji": "🎊",
      "Meaning": "Confetti ball",
      "Unicode": "U+1F38A"
    },
    {
      "Emoji": "🎋",
      "Meaning": "Tanabata tree",
      "Unicode": "U+1F38B"
    },
    {
      "Emoji": "🎍",
      "Meaning": "Pine decoration",
      "Unicode": "U+1F38D"
    },
    {
      "Emoji": "🎎",
      "Meaning": "Japanese dolls",
      "Unicode": "U+1F38E"
    },
    {
      "Emoji": "🎏",
      "Meaning": "Carp streamer",
      "Unicode": "U+1F38F"
    },
    {
      "Emoji": "🎑",
      "Meaning": "Moon viewing ceremony",
      "Unicode": "U+1F391"
    },
    {
      "Emoji": "🧧",
      "Meaning": "Red envelope",
      "Unicode": "U+1F9E7"
    },
    {
      "Emoji": "🎀",
      "Meaning": "Ribbon",
      "Unicode": "U+1F380"
    },
    {
      "Emoji": "🎁",
      "Meaning": "Wrapped gift",
      "Unicode": "U+1F381"
    },
    {
      "Emoji": "🎗",
      "Meaning": "Reminder ribbon",
      "Unicode": "U+1F397"
    },
    {
      "Emoji": "🎟",
      "Meaning": "Admission ticket",
      "Unicode": "U+1F39F"
    },
    {
      "Emoji": "🎫",
      "Meaning": "Ticket",
      "Unicode": "U+1F3AB"
    },
    {
      "Emoji": "🎖",
      "Meaning": "Military medal",
      "Unicode": "U+1F396"
    },
    {
      "Emoji": "🏆",
      "Meaning": "Trophy",
      "Unicode": "U+1F3C6"
    },
    {
      "Emoji": "🏅",
      "Meaning": "Sports medal",
      "Unicode": "U+1F3C5"
    },
    {
      "Emoji": "🥇",
      "Meaning": "Gold medal - first position",
      "Unicode": "U+1F947"
    },
    {
      "Emoji": "🥈",
      "Meaning": "Silver medal - second position",
      "Unicode": "U+1F948"
    },
    {
      "Emoji": "🥉",
      "Meaning": "Bronze medal - third position",
      "Unicode": "U+1F949"
    },
    {
      "Emoji": "⚽",
      "Meaning": "Soccer ball",
      "Unicode": "U+26BD"
    },
    {
      "Emoji": "⚾",
      "Meaning": "Baseball",
      "Unicode": "U+26BE"
    },
    {
      "Emoji": "🥎",
      "Meaning": "Softball",
      "Unicode": "U+1F94E"
    },
    {
      "Emoji": "🏀",
      "Meaning": "BAsketball",
      "Unicode": "U+1F3C0"
    },
    {
      "Emoji": "🏐",
      "Meaning": "Volleyball",
      "Unicode": "U+1F3D0"
    },
    {
      "Emoji": "🏈",
      "Meaning": "American football",
      "Unicode": "U+1F3C8"
    },
    {
      "Emoji": "🏉",
      "Meaning": "Rugby",
      "Unicode": "U+1F3C9"
    },
    {
      "Emoji": "🎾",
      "Meaning": "Tennis",
      "Unicode": "U+1F3BE"
    },
    {
      "Emoji": "🥏",
      "Meaning": "Flying disk",
      "Unicode": "U+1F94F"
    },
    {
      "Emoji": "🎳",
      "Meaning": "Bowling",
      "Unicode": "U+1F3B3"
    },
    {
      "Emoji": "🏏",
      "Meaning": "Cricket",
      "Unicode": "U+1F3CF"
    },
    {
      "Emoji": "🏑",
      "Meaning": "Field hockey",
      "Unicode": "U+1F3D1"
    },
    {
      "Emoji": "🏒",
      "Meaning": "Ice hockey",
      "Unicode": "U+1F3D2"
    },
    {
      "Emoji": "🥍",
      "Meaning": "Lacrose",
      "Unicode": "U+1F94D"
    },
    {
      "Emoji": "🏓",
      "Meaning": "Ping pong",
      "Unicode": "U+1F3D3"
    },
    {
      "Emoji": "🏸",
      "Meaning": "Badminton",
      "Unicode": "U+1F3F8"
    },
    {
      "Emoji": "🥊",
      "Meaning": "Boxing glove",
      "Unicode": "U+1F94A"
    },
    {
      "Emoji": "🥋",
      "Meaning": "Martial arts uniform",
      "Unicode": "U+1F94B"
    },
    {
      "Emoji": "🥅",
      "Meaning": "Goal net",
      "Unicode": "U+1F945"
    },
    {
      "Emoji": "⛳",
      "Meaning": "Flag in a hole",
      "Unicode": "U+26F3"
    },
    {
      "Emoji": "⛸",
      "Meaning": "Ice skate",
      "Unicode": "U+26F8"
    },
    {
      "Emoji": "🎣",
      "Meaning": "Fishing poll",
      "Unicode": "U+1F3A3"
    },
    {
      "Emoji": "🤿",
      "Meaning": "Driving mask",
      "Unicode": "U+1F93F"
    },
    {
      "Emoji": "🎽",
      "Meaning": "Running shirt",
      "Unicode": "U+1F3BD"
    },
    {
      "Emoji": "🎿",
      "Meaning": "Skis",
      "Unicode": "U+1F3BF"
    },
    {
      "Emoji": "🛷",
      "Meaning": "Sled",
      "Unicode": "U+1F6F7"
    },
    {
      "Emoji": "🥌",
      "Meaning": "Curling stone",
      "Unicode": "U+1F94C"
    },
    {
      "Emoji": "🎯",
      "Meaning": "Bullseye",
      "Unicode": "U+1F3AF"
    },
    {
      "Emoji": "🪀",
      "Meaning": "Yo-yo",
      "Unicode": "U+1FA80"
    },
    {
      "Emoji": "🪁",
      "Meaning": "Kite",
      "Unicode": "U+1FA81"
    },
    {
      "Emoji": "🎱",
      "Meaning": "8 ball",
      "Unicode": "U+1F3B1"
    },
    {
      "Emoji": "🔮",
      "Meaning": "Crystal ball",
      "Unicode": "U+1F52E"
    },
    {
      "Emoji": "🪄",
      "Meaning": "Magic wand",
      "Unicode": "U+1FA84"
    },
    {
      "Emoji": "🧿",
      "Meaning": "Nazar amulet",
      "Unicode": "U+1F9FF"
    },
    {
      "Emoji": "🪄",
      "Meaning": "Hamsa",
      "Unicode": "U+1FAAC"
    },
    {
      "Emoji": "🎮",
      "Meaning": "Video game pad",
      "Unicode": "U+1F3AE"
    },
    {
      "Emoji": "🕹",
      "Meaning": "Joystick",
      "Unicode": "U+1F579"
    },
    {
      "Emoji": "🎰",
      "Meaning": "Slot machine",
      "Unicode": "U+1F3B0"
    },
    {
      "Emoji": "🎲",
      "Meaning": "Game die",
      "Unicode": "U+1F3B2"
    },
    {
      "Emoji": "🧩",
      "Meaning": "Puxxle piece",
      "Unicode": "U+1F9E9"
    },
    {
      "Emoji": "🧸",
      "Meaning": "Teddy bear",
      "Unicode": "U+1F9F8"
    },
    {
      "Emoji": "🪅",
      "Meaning": "Pinata",
      "Unicode": "U+1FA85"
    },
    {
      "Emoji": "🪆",
      "Meaning": "Mirror",
      "Unicode": "U+1FAA9"
    },
    {
      "Emoji": "🪆",
      "Meaning": "Nesting doll",
      "Unicode": "U+1FA86"
    },
    {
      "Emoji": "♠",
      "Meaning": "Spade suit",
      "Unicode": "U+2660"
    },
    {
      "Emoji": "♥",
      "Meaning": "Heart suit",
      "Unicode": "U+2665"
    },
    {
      "Emoji": "♣",
      "Meaning": "Club suit",
      "Unicode": "U+2663"
    },
    {
      "Emoji": "♟",
      "Meaning": "Chess pawn",
      "Unicode": "U+265F"
    },
    {
      "Emoji": "🃏",
      "Meaning": "Joker",
      "Unicode": "U+1F0CF"
    },
    {
      "Emoji": "🀄",
      "Meaning": "Mahjong red dragon",
      "Unicode": "U+1F004"
    },
    {
      "Emoji": "🎴",
      "Meaning": "Flower playing cards",
      "Unicode": "U+1F3B4"
    },
    {
      "Emoji": "🎭",
      "Meaning": "Performing arts",
      "Unicode": "U+1F3AD"
    },
    {
      "Emoji": "🖼",
      "Meaning": "Framed picture",
      "Unicode": "U+1F5BC"
    },
    {
      "Emoji": "🎨",
      "Meaning": "Artist pallete",
      "Unicode": "U+1F3A8"
    },
    {
      "Emoji": "🧵",
      "Meaning": "Thread",
      "Unicode": "U+1F9F5"
    },
    {
      "Emoji": "🪡",
      "Meaning": "Sewing needle with thred",
      "Unicode": "U+1FAA1"
    },
    {
      "Emoji": "🧶",
      "Meaning": "Yarn",
      "Unicode": "U+1F9F6"
    },
    {
      "Emoji": "🪢",
      "Meaning": "Knot",
      "Unicode": "U+1FAA2"
    },
    {
      "Emoji": "👓",
      "Meaning": "Glasses",
      "Unicode": "U+1F453"
    },
    {
      "Emoji": "🕶",
      "Meaning": "Sunglasses",
      "Unicode": "U+1F576"
    },
    {
      "Emoji": "🥽",
      "Meaning": "Googles",
      "Unicode": "U+1F97D"
    },
    {
      "Emoji": "🥼",
      "Meaning": "Lab coat",
      "Unicode": "U+1F97C"
    },
    {
      "Emoji": "🦺",
      "Meaning": "Safety vest",
      "Unicode": "U+1F9BA"
    },
    {
      "Emoji": "👔",
      "Meaning": "Necktie",
      "Unicode": "U+1F454"
    },
    {
      "Emoji": "👕",
      "Meaning": "T-shirt",
      "Unicode": "U+1F455"
    },
    {
      "Emoji": "👖",
      "Meaning": "Jeans",
      "Unicode": "U+1F456"
    },
    {
      "Emoji": "🧣",
      "Meaning": "Scarf",
      "Unicode": "U+1F9E3"
    },
    {
      "Emoji": "🧤",
      "Meaning": "Gloves",
      "Unicode": "U+1F9E4"
    },
    {
      "Emoji": "🧥",
      "Meaning": "Coat",
      "Unicode": "U+1F9E5"
    },
    {
      "Emoji": "🧦",
      "Meaning": "Socks",
      "Unicode": "U+1F9E6"
    },
    {
      "Emoji": "👗",
      "Meaning": "Dress",
      "Unicode": "U+1F457"
    },
    {
      "Emoji": "👘",
      "Meaning": "Kimono",
      "Unicode": "U+1F458"
    },
    {
      "Emoji": "🥻",
      "Meaning": "Sari",
      "Unicode": "U+1F97B"
    },
    {
      "Emoji": "🩱",
      "Meaning": "One piece suit",
      "Unicode": "U+1FA71"
    },
    {
      "Emoji": "🩲",
      "Meaning": "Briefs",
      "Unicode": "U+1FA72"
    },
    {
      "Emoji": "🩳",
      "Meaning": "Shorts",
      "Unicode": "U+1FA73"
    },
    {
      "Emoji": "👙",
      "Meaning": "Bikini",
      "Unicode": "U+1F459"
    },
    {
      "Emoji": "👚",
      "Meaning": "Woman's cloth",
      "Unicode": "U+1F45A"
    },
    {
      "Emoji": "👛",
      "Meaning": "Purse",
      "Unicode": "U+1F45B"
    },
    {
      "Emoji": "👜",
      "Meaning": "Handbag",
      "Unicode": "U+1F45C"
    },
    {
      "Emoji": "👝",
      "Meaning": "Clutch bag",
      "Unicode": "U+1F45D"
    },
    {
      "Emoji": "🛍",
      "Meaning": "Shopping bags",
      "Unicode": "U+1F6CD"
    },
    {
      "Emoji": "🎒",
      "Meaning": "Backpack",
      "Unicode": "U+1F392"
    },
    {
      "Emoji": "🩴",
      "Meaning": "Thong sandals",
      "Unicode": "U+1FA74"
    },
    {
      "Emoji": "👞",
      "Meaning": "Man's shoe",
      "Unicode": "U+1F45E"
    },
    {
      "Emoji": "👟",
      "Meaning": "Running shoe",
      "Unicode": "U+1F45F"
    },
    {
      "Emoji": "🥾",
      "Meaning": "Hiking boot",
      "Unicode": "U+1F97E"
    },
    {
      "Emoji": "🥿",
      "Meaning": "Flat shoe",
      "Unicode": "U+1F97F"
    },
    {
      "Emoji": "👠",
      "Meaning": "High-heeled shoe",
      "Unicode": "U+1F460"
    },
    {
      "Emoji": "👡",
      "Meaning": "Woman's sandal",
      "Unicode": "U+1F461"
    },
    {
      "Emoji": "🩰",
      "Meaning": "Ballet shoes",
      "Unicode": "U+1FA70"
    },
    {
      "Emoji": "👢",
      "Meaning": "Woman's boot",
      "Unicode": "U+1F462"
    },
    {
      "Emoji": "👑",
      "Meaning": "Crown",
      "Unicode": "U+1F451"
    },
    {
      "Emoji": "👒",
      "Meaning": "Woman's hat",
      "Unicode": "U+1F452"
    },
    {
      "Emoji": "🎩",
      "Meaning": "Top hat",
      "Unicode": "U+1F3A9"
    },
    {
      "Emoji": "🎓",
      "Meaning": "Graduation cap",
      "Unicode": "U+1F393"
    },
    {
      "Emoji": "🧢",
      "Meaning": "Billed cap",
      "Unicode": "U+1F9E2"
    },
    {
      "Emoji": "🪖",
      "Meaning": "Military helmet",
      "Unicode": "U+1FA96"
    },
    {
      "Emoji": "⛑",
      "Meaning": "Rescuew worker's helmet",
      "Unicode": "U+26D1"
    },
    {
      "Emoji": "📿",
      "Meaning": "PRayer beads",
      "Unicode": "U+1F4FF"
    },
    {
      "Emoji": "💄",
      "Meaning": "Lipstick",
      "Unicode": "U+1F484"
    },
    {
      "Emoji": "💍",
      "Meaning": "Ring",
      "Unicode": "U+1F48D"
    },
    {
      "Emoji": "💎",
      "Meaning": "Gemstone",
      "Unicode": "U+1F48E"
    },
    {
      "Emoji": "🔇",
      "Meaning": "Muted speaker",
      "Unicode": "U+1F507"
    },
    {
      "Emoji": "🔈",
      "Meaning": "Low volume speaker",
      "Unicode": "U+1F508"
    },
    {
      "Emoji": "🔉",
      "Meaning": "Mid volume speaker",
      "Unicode": "U+1F509"
    },
    {
      "Emoji": "🔊",
      "Meaning": "High volume speaker",
      "Unicode": "U+1F50A"
    },
    {
      "Emoji": "📢",
      "Meaning": "Loudspeaker",
      "Unicode": "U+1F4E2"
    },
    {
      "Emoji": "📣",
      "Meaning": "Megaphone",
      "Unicode": "U+1F4E3"
    },
    {
      "Emoji": "📯",
      "Meaning": "Postal horn",
      "Unicode": "U+1F4EF"
    },
    {
      "Emoji": "🔔",
      "Meaning": "Bell",
      "Unicode": "U+1F514"
    },
    {
      "Emoji": "🔕",
      "Meaning": "Bell with slash",
      "Unicode": "U+1F515"
    },
    {
      "Emoji": "🎼",
      "Meaning": "Musical score",
      "Unicode": "U+1F3BC"
    },
    {
      "Emoji": "🎵",
      "Meaning": "Musical note",
      "Unicode": "U+1F3B5"
    },
    {
      "Emoji": "🎶",
      "Meaning": "Musical notes",
      "Unicode": "U+1F3B6"
    },
    {
      "Emoji": "🎙",
      "Meaning": "Studio microphone",
      "Unicode": "U+1F399"
    },
    {
      "Emoji": "🎚",
      "Meaning": "Level slider",
      "Unicode": "U+1F39A"
    },
    {
      "Emoji": "🎛",
      "Meaning": "Control knobs",
      "Unicode": "U+1F39B"
    },
    {
      "Emoji": "🎤",
      "Meaning": "Microphone",
      "Unicode": "U+1F3A4"
    },
    {
      "Emoji": "🎧",
      "Meaning": "Headphone",
      "Unicode": "U+1F3A7"
    },
    {
      "Emoji": "📻",
      "Meaning": "Radio",
      "Unicode": "U+1F4FB"
    },
    {
      "Emoji": "🎷",
      "Meaning": "Saxophone",
      "Unicode": "U+1F3B7"
    },
    {
      "Emoji": "🪗",
      "Meaning": "Accordion",
      "Unicode": "U+1FA97"
    },
    {
      "Emoji": "🎸",
      "Meaning": "Guitar",
      "Unicode": "U+1F3B8"
    },
    {
      "Emoji": "🎹",
      "Meaning": "Musical keyboard",
      "Unicode": "U+1F3B9"
    },
    {
      "Emoji": "🎺",
      "Meaning": "Trumpet",
      "Unicode": "U+1F3BA"
    },
    {
      "Emoji": "🎻",
      "Meaning": "Violin",
      "Unicode": "U+1F3BB"
    },
    {
      "Emoji": "🪕",
      "Meaning": "Banjo",
      "Unicode": "U+1FA95"
    },
    {
      "Emoji": "🥁",
      "Meaning": "Drum",
      "Unicode": "U+1F941"
    },
    {
      "Emoji": "🪘",
      "Meaning": "Long drum",
      "Unicode": "U+1FA98"
    },
    {
      "Emoji": "📱",
      "Meaning": "Mobile phone",
      "Unicode": "U+1F4F1"
    },
    {
      "Emoji": "📲",
      "Meaning": "MObile phone with arrow",
      "Unicode": "U+1F4F2"
    },
    {
      "Emoji": "☎",
      "Meaning": "Telephone",
      "Unicode": "U+260E"
    },
    {
      "Emoji": "📞",
      "Meaning": "Telephone receiver",
      "Unicode": "U+1F4DE"
    },
    {
      "Emoji": "📟",
      "Meaning": "Pager",
      "Unicode": "U+1F4DF"
    },
    {
      "Emoji": "📠",
      "Meaning": "Fax machine",
      "Unicode": "U+1F4E0"
    },
    {
      "Emoji": "🔋",
      "Meaning": "Full battery",
      "Unicode": "U+1F50B"
    },
    {
      "Emoji": "🪫",
      "Meaning": "Low battery",
      "Unicode": "U+1FAAB"
    },
    {
      "Emoji": "🔌",
      "Meaning": "Electric plug",
      "Unicode": "U+1F50C"
    },
    {
      "Emoji": "💻",
      "Meaning": "Laptop",
      "Unicode": "U+1F4BB"
    },
    {
      "Emoji": "🖥",
      "Meaning": "Desktop computer",
      "Unicode": "U+1F5A5"
    },
    {
      "Emoji": "🖨",
      "Meaning": "Printer",
      "Unicode": "U+1F5A8"
    },
    {
      "Emoji": "⌨",
      "Meaning": "Keyboard",
      "Unicode": "U+2328"
    },
    {
      "Emoji": "🖱",
      "Meaning": "Mouse",
      "Unicode": "U+1F5B1"
    },
    {
      "Emoji": "🖲",
      "Meaning": "Trackball",
      "Unicode": "U+1F5B2"
    },
    {
      "Emoji": "💽",
      "Meaning": "Computer disk",
      "Unicode": "U+1F4BD"
    },
    {
      "Emoji": "💾",
      "Meaning": "Floppy disk",
      "Unicode": "U+1F4BE"
    },
    {
      "Emoji": "💿",
      "Meaning": "Optical disk",
      "Unicode": "U+1F4BF"
    },
    {
      "Emoji": "📀",
      "Meaning": "DVD",
      "Unicode": "U+1F4C0"
    },
    {
      "Emoji": "🧮",
      "Meaning": "Abacus",
      "Unicode": "U+1F9EE"
    },
    {
      "Emoji": "🎥",
      "Meaning": "Movie camera",
      "Unicode": "U+1F3A5"
    },
    {
      "Emoji": "🎞",
      "Meaning": "Film frames",
      "Unicode": "U+1F39E"
    },
    {
      "Emoji": "📽",
      "Meaning": "Film Projector",
      "Unicode": "U+1F4FD"
    },
    {
      "Emoji": "🎬",
      "Meaning": "Clapper board",
      "Unicode": "U+1F3AC"
    },
    {
      "Emoji": "📺",
      "Meaning": "Television",
      "Unicode": "U+1F4FA"
    },
    {
      "Emoji": "📷",
      "Meaning": "Camera",
      "Unicode": "U+1F4F7"
    },
    {
      "Emoji": "📸",
      "Meaning": "Camera with flash",
      "Unicode": "U+1F4F8"
    },
    {
      "Emoji": "📹",
      "Meaning": "Video camera",
      "Unicode": "U+1F4F9"
    },
    {
      "Emoji": "📼",
      "Meaning": "Video cassete",
      "Unicode": "U+1F4FC"
    },
    {
      "Emoji": "🔍",
      "Meaning": "Magnifying glass tilted left",
      "Unicode": "U+1F50D"
    },
    {
      "Emoji": "🔎",
      "Meaning": "Magnifying glass tilted right",
      "Unicode": "U+1F50E"
    },
    {
      "Emoji": "🕯",
      "Meaning": "Candle",
      "Unicode": "U+1F56F"
    },
    {
      "Emoji": "💡",
      "Meaning": "Light bulb",
      "Unicode": "U+1F4A1"
    },
    {
      "Emoji": "🔦",
      "Meaning": "Flashlight",
      "Unicode": "U+1F526"
    },
    {
      "Emoji": "🏮",
      "Meaning": "Red pepper lantern",
      "Unicode": "U+1F3EE"
    },
    {
      "Emoji": "🪔",
      "Meaning": "Diya lamp",
      "Unicode": "U+1FA94"
    },
    {
      "Emoji": "📔",
      "Meaning": "Notebook with decorative cover",
      "Unicode": "U+1F4D4"
    },
    {
      "Emoji": "📕",
      "Meaning": "Closed notebook",
      "Unicode": "U+1F4D5"
    },
    {
      "Emoji": "📖",
      "Meaning": "Opened notebook",
      "Unicode": "U+1F4D6"
    },
    {
      "Emoji": "📗",
      "Meaning": "Green book",
      "Unicode": "U+1F4D7"
    },
    {
      "Emoji": "📘",
      "Meaning": "Blue book",
      "Unicode": "U+1F4D8"
    },
    {
      "Emoji": "📙",
      "Meaning": "Orange book",
      "Unicode": "U+1F4D9"
    },
    {
      "Emoji": "📚",
      "Meaning": "Orange books",
      "Unicode": "U+1F4DA"
    },
    {
      "Emoji": "📓",
      "Meaning": "Notebook",
      "Unicode": "U+1F4D3"
    },
    {
      "Emoji": "📒",
      "Meaning": "Ledger",
      "Unicode": "U+1F4D2"
    },
    {
      "Emoji": "📃",
      "Meaning": "Page with curl",
      "Unicode": "U+1F4C3"
    },
    {
      "Emoji": "📜",
      "Meaning": "Scroll",
      "Unicode": "U+1F4DC"
    },
    {
      "Emoji": "📄",
      "Meaning": "Page facing up",
      "Unicode": "U+1F4C4"
    },
    {
      "Emoji": "📰",
      "Meaning": "Newspaper",
      "Unicode": "U+1F4F0"
    },
    {
      "Emoji": "🗞",
      "Meaning": "Rolled-up newspaper",
      "Unicode": "U+1F5DE"
    },
    {
      "Emoji": "📑",
      "Meaning": "Bookmark tabs",
      "Unicode": "U+1F4D1"
    },
    {
      "Emoji": "🔖",
      "Meaning": "Bookmark",
      "Unicode": "U+1F516"
    },
    {
      "Emoji": "🏷",
      "Meaning": "Label",
      "Unicode": "U+1F3F7"
    },
    {
      "Emoji": "💰",
      "Meaning": "Money bag",
      "Unicode": "U+1F4B0"
    },
    {
      "Emoji": "🪙",
      "Meaning": "Coin",
      "Unicode": "U+1FA99"
    },
    {
      "Emoji": "💴",
      "Meaning": "Yen banknote",
      "Unicode": "U+1F4B4"
    },
    {
      "Emoji": "💵",
      "Meaning": "Dollar banknote",
      "Unicode": "U+1F4B5"
    },
    {
      "Emoji": "💶",
      "Meaning": "Euro banknote",
      "Unicode": "U+1F4B6"
    },
    {
      "Emoji": "💷",
      "Meaning": "Pound banknote",
      "Unicode": "U+1F4B7"
    },
    {
      "Emoji": "💸",
      "Meaning": "Money with wings",
      "Unicode": "U+1F4B8"
    },
    {
      "Emoji": "💳",
      "Meaning": "Credit card",
      "Unicode": "U+1F4B3"
    },
    {
      "Emoji": "🧾",
      "Meaning": "Receipt",
      "Unicode": "U+1F9FE"
    },
    {
      "Emoji": "💹",
      "Meaning": "Chart increase woth Yen",
      "Unicode": "U+1F4B9"
    },
    {
      "Emoji": "✉",
      "Meaning": "Envelope",
      "Unicode": "U+2709"
    },
    {
      "Emoji": "📧",
      "Meaning": "e-mail",
      "Unicode": "U+1F4E7"
    },
    {
      "Emoji": "📩",
      "Meaning": "Envelope with arrow",
      "Unicode": "U+1F4E9"
    },
    {
      "Emoji": "📤",
      "Meaning": "Outbox tray",
      "Unicode": "U+1F4E4"
    },
    {
      "Emoji": "📥",
      "Meaning": "Inbox tray",
      "Unicode": "U+1F4E5"
    },
    {
      "Emoji": "📦",
      "Meaning": "Package",
      "Unicode": "U+1F4E6"
    },
    {
      "Emoji": "📫",
      "Meaning": "Closed mailbox with raised flag",
      "Unicode": "U+1F4EB"
    },
    {
      "Emoji": "📪",
      "Meaning": "Closed mailbox with lowered flag",
      "Unicode": "U+1F4EA"
    },
    {
      "Emoji": "📬",
      "Meaning": "Open mailbox with raised flag",
      "Unicode": "U+1F4EC"
    },
    {
      "Emoji": "📭",
      "Meaning": "Open mailbox with lowered flag",
      "Unicode": "U+1F4ED"
    },
    {
      "Emoji": "📮",
      "Meaning": "Postbox",
      "Unicode": "U+1F4EE"
    },
    {
      "Emoji": "🗳",
      "Meaning": "Ballot box with ballot",
      "Unicode": "U+1F5F3"
    },
    {
      "Emoji": "✏",
      "Meaning": "Pencil",
      "Unicode": "U+270F"
    },
    {
      "Emoji": "✒",
      "Meaning": "Black nib",
      "Unicode": "U+2712"
    },
    {
      "Emoji": "🖋",
      "Meaning": "Fountain pen",
      "Unicode": "U+1F58B"
    },
    {
      "Emoji": "🖊",
      "Meaning": "Pen",
      "Unicode": "U+1F58A"
    },
    {
      "Emoji": "🖌",
      "Meaning": "Paintbrush",
      "Unicode": "U+1F58C"
    },
    {
      "Emoji": "🖍",
      "Meaning": "Crayon",
      "Unicode": "U+1F58D"
    },
    {
      "Emoji": "📝",
      "Meaning": "Memo",
      "Unicode": "U+1F4DD"
    },
    {
      "Emoji": "💼",
      "Meaning": "Briefcase",
      "Unicode": "U+1F4BC"
    },
    {
      "Emoji": "📁",
      "Meaning": "File folder",
      "Unicode": "U+1F4C1"
    },
    {
      "Emoji": "📂",
      "Meaning": "Open the folder",
      "Unicode": "U+1F4C2"
    },
    {
      "Emoji": "🗂",
      "Meaning": "Card index dividers",
      "Unicode": "U+1F5C2"
    },
    {
      "Emoji": "📅",
      "Meaning": "Calender",
      "Unicode": "U+1F4C5"
    },
    {
      "Emoji": "📆",
      "Meaning": "Tear off calender",
      "Unicode": "U+1F4C6"
    },
    {
      "Emoji": "📇",
      "Meaning": "Card index",
      "Unicode": "U+1F4C7"
    },
    {
      "Emoji": "📈",
      "Meaning": "Increasing chart",
      "Unicode": "U+1F4C8"
    },
    {
      "Emoji": "📉",
      "Meaning": "Decreasing chart",
      "Unicode": "U+1F4C9"
    },
    {
      "Emoji": "📊",
      "Meaning": "Bar chart",
      "Unicode": "U+1F4CA"
    },
    {
      "Emoji": "📋",
      "Meaning": "Clipboard",
      "Unicode": "U+1F4CB"
    },
    {
      "Emoji": "📌",
      "Meaning": "Pushpin",
      "Unicode": "U+1F4CC"
    },
    {
      "Emoji": "📍",
      "Meaning": "Round pushpin",
      "Unicode": "U+1F4CD"
    },
    {
      "Emoji": "📎",
      "Meaning": "Paperclip",
      "Unicode": "U+1F4CE"
    },
    {
      "Emoji": "🖇",
      "Meaning": "Linked paperclips",
      "Unicode": "U+1F587"
    },
    {
      "Emoji": "📏",
      "Meaning": "Straight ruler",
      "Unicode": "U+1F4CF"
    },
    {
      "Emoji": "📐",
      "Meaning": "Triangular ruler",
      "Unicode": "U+1F4D0"
    },
    {
      "Emoji": "✂",
      "Meaning": "Scissors",
      "Unicode": "U+2702"
    },
    {
      "Emoji": "🗃",
      "Meaning": "Card file box",
      "Unicode": "U+1F5C3"
    },
    {
      "Emoji": "🗄",
      "Meaning": "File cabinet",
      "Unicode": "U+1F5C4"
    },
    {
      "Emoji": "🗑",
      "Meaning": "Waste basket",
      "Unicode": "U+1F5D1"
    },
    {
      "Emoji": "🔒",
      "Meaning": "Locked",
      "Unicode": "U+1F512"
    },
    {
      "Emoji": "🔓",
      "Meaning": "Unlocked",
      "Unicode": "U+1F513"
    },
    {
      "Emoji": "🔏",
      "Meaning": "Locked with pen",
      "Unicode": "U+1F50F"
    },
    {
      "Emoji": "🔐",
      "Meaning": "Locked with key",
      "Unicode": "U+1F510"
    },
    {
      "Emoji": "🔑",
      "Meaning": "Key",
      "Unicode": "U+1F511"
    },
    {
      "Emoji": "🗝",
      "Meaning": "Old key",
      "Unicode": "U+1F5DD"
    },
    {
      "Emoji": "🔨",
      "Meaning": "Hammer",
      "Unicode": "U+1F528"
    },
    {
      "Emoji": "🪓",
      "Meaning": "Axe",
      "Unicode": "U+1FA93"
    },
    {
      "Emoji": "⛏",
      "Meaning": "Pick",
      "Unicode": "U+26CF"
    },
    {
      "Emoji": "⚒",
      "Meaning": "Hammer and pick",
      "Unicode": "U+2692"
    },
    {
      "Emoji": "🛠",
      "Meaning": "Hammer and wrench",
      "Unicode": "U+1F6E0"
    },
    {
      "Emoji": "🗡",
      "Meaning": "Sword",
      "Unicode": "U+1F5E1"
    },
    {
      "Emoji": "⚔",
      "Meaning": "Crossed swords",
      "Unicode": "U+2694"
    },
    {
      "Emoji": "🔫",
      "Meaning": "Water gun",
      "Unicode": "U+1F52B"
    },
    {
      "Emoji": "🪃",
      "Meaning": "Boomerang",
      "Unicode": "U+1FA83"
    },
    {
      "Emoji": "🏹",
      "Meaning": "Bow and arrow",
      "Unicode": "U+1F3F9"
    },
    {
      "Emoji": "🛡",
      "Meaning": "Shield",
      "Unicode": "U+1F6E1"
    },
    {
      "Emoji": "🪚",
      "Meaning": "Carpentry saw",
      "Unicode": "U+1FA9A"
    },
    {
      "Emoji": "🔧",
      "Meaning": "Wrench",
      "Unicode": "U+1F527"
    },
    {
      "Emoji": "🪛",
      "Meaning": "Screwdriver",
      "Unicode": "U+1FA9B"
    },
    {
      "Emoji": "🔩",
      "Meaning": "Bolt and nut",
      "Unicode": "U+1F529"
    },
    {
      "Emoji": "⚙",
      "Meaning": "Wheel",
      "Unicode": "U+2699"
    },
    {
      "Emoji": "🗜",
      "Meaning": "Clamp",
      "Unicode": "U+1F5DC"
    },
    {
      "Emoji": "⚖",
      "Meaning": "Balance scale",
      "Unicode": "U+2696"
    },
    {
      "Emoji": "🦯",
      "Meaning": "White cane",
      "Unicode": "U+1F9AF"
    },
    {
      "Emoji": "🔗",
      "Meaning": "Link",
      "Unicode": "U+1F517"
    },
    {
      "Emoji": "⛓",
      "Meaning": "Chains",
      "Unicode": "U+26D3"
    },
    {
      "Emoji": "🪝",
      "Meaning": "Hook",
      "Unicode": "U+1FA9D"
    },
    {
      "Emoji": "🧰",
      "Meaning": "Toolbox",
      "Unicode": "U+1F9F0"
    },
    {
      "Emoji": "🧲",
      "Meaning": "Magnet",
      "Unicode": "U+1F9F2"
    },
    {
      "Emoji": "🪜",
      "Meaning": "Ladder",
      "Unicode": "U+1FA9C"
    },
    {
      "Emoji": "⚗",
      "Meaning": "Alembic",
      "Unicode": "U+2697"
    },
    {
      "Emoji": "🧪",
      "Meaning": "Test tube",
      "Unicode": "U+1F9EA"
    },
    {
      "Emoji": "🧫",
      "Meaning": "Petri dish",
      "Unicode": "U+1F9EB"
    },
    {
      "Emoji": "🧬",
      "Meaning": "DNA",
      "Unicode": "U+1F9EC"
    },
    {
      "Emoji": "🔬",
      "Meaning": "Microscope",
      "Unicode": "U+1F52C"
    },
    {
      "Emoji": "🔭",
      "Meaning": "Telescope",
      "Unicode": "U+1F52D"
    },
    {
      "Emoji": "📡",
      "Meaning": "Satelite antenna",
      "Unicode": "U+1F4E1"
    },
    {
      "Emoji": "💉",
      "Meaning": "Syringe",
      "Unicode": "U+1F489"
    },
    {
      "Emoji": "🩸",
      "Meaning": "A droplet of blood",
      "Unicode": "U+1FA78"
    },
    {
      "Emoji": "💊",
      "Meaning": "Pill",
      "Unicode": "U+1F48A"
    },
    {
      "Emoji": "🩹",
      "Meaning": "Adhesive bandage",
      "Unicode": "🩹"
    },
    {
      "Emoji": "🩼",
      "Meaning": "Clutch",
      "Unicode": "U+1FA7C"
    },
    {
      "Emoji": "🩺",
      "Meaning": "Stethoscope",
      "Unicode": "U+1FA7A"
    },
    {
      "Emoji": "🚪",
      "Meaning": "Door",
      "Unicode": "U+1F6AA"
    },
    {
      "Emoji": "🛗",
      "Meaning": "Elevator",
      "Unicode": "U+1F6D7"
    },
    {
      "Emoji": "🪞",
      "Meaning": "Mirror",
      "Unicode": "U+1FA9E"
    },
    {
      "Emoji": "🪟",
      "Meaning": "Window",
      "Unicode": "U+1FA9F"
    },
    {
      "Emoji": "🛏",
      "Meaning": "Bed",
      "Unicode": "U+1F6CF"
    },
    {
      "Emoji": "🛋",
      "Meaning": "Couch and lamp",
      "Unicode": "U+1F6CB"
    },
    {
      "Emoji": "🪑",
      "Meaning": "Chair",
      "Unicode": "U+1FA91"
    },
    {
      "Emoji": "🚽",
      "Meaning": "Toilet",
      "Unicode": "U+1F6BD"
    },
    {
      "Emoji": "🪠",
      "Meaning": "Plunger",
      "Unicode": "U+1FAA0"
    },
    {
      "Emoji": "🚿",
      "Meaning": "Shower",
      "Unicode": "U+1F6BF"
    },
    {
      "Emoji": "🛁",
      "Meaning": "Bathtub",
      "Unicode": "U+1F6C1"
    },
    {
      "Emoji": "🪤",
      "Meaning": "Mouse trap",
      "Unicode": "U+1FAA4"
    },
    {
      "Emoji": "🪒",
      "Meaning": "Razor",
      "Unicode": "U+1FA92"
    },
    {
      "Emoji": "🧴",
      "Meaning": "Lotion bottle",
      "Unicode": "U+1F9F4"
    },
    {
      "Emoji": "🧷",
      "Meaning": "Safety pin",
      "Unicode": "U+1F9F7"
    },
    {
      "Emoji": "🧹",
      "Meaning": "Broom",
      "Unicode": "U+1F9F9"
    },
    {
      "Emoji": "🧺",
      "Meaning": "Basket",
      "Unicode": "U+1F9FA"
    },
    {
      "Emoji": "🧻",
      "Meaning": "Roll of paper",
      "Unicode": "U+1F9FB"
    },
    {
      "Emoji": "🪣",
      "Meaning": "Bucket",
      "Unicode": "U+1FAA3"
    },
    {
      "Emoji": "🧼",
      "Meaning": "Soap",
      "Unicode": "U+1F9FC"
    },
    {
      "Emoji": "🫧",
      "Meaning": "Bubbles",
      "Unicode": "U+1FAE7"
    },
    {
      "Emoji": "🪥",
      "Meaning": "Toothbrush",
      "Unicode": "U+1FAA5"
    },
    {
      "Emoji": "🧽",
      "Meaning": "Sponge",
      "Unicode": "U+1F9FD"
    },
    {
      "Emoji": "🧯",
      "Meaning": "Fire extinguisher",
      "Unicode": "U+1F9EF"
    },
    {
      "Emoji": "🛒",
      "Meaning": "Shopping cart",
      "Unicode": "U+1F6D2"
    },
    {
      "Emoji": "🚬",
      "Meaning": "Cigarette",
      "Unicode": "U+1F6AC"
    },
    {
      "Emoji": "⚰",
      "Meaning": "Casket",
      "Unicode": "U+26B0"
    },
    {
      "Emoji": "🪦",
      "Meaning": "Headstone",
      "Unicode": "U+1FAA6"
    },
    {
      "Emoji": "⚱",
      "Meaning": "Funeral urn",
      "Unicode": "U+26B1"
    },
    {
      "Emoji": "🗿",
      "Meaning": "Mole",
      "Unicode": "U+1F5FF"
    },
    {
      "Emoji": "🪧",
      "Meaning": "Placard",
      "Unicode": "U+1FAA7"
    },
    {
      "Emoji": "🏧",
      "Meaning": "ATM Sign",
      "Unicode": "U+1F3E7"
    },
    {
      "Emoji": "🚮",
      "Meaning": "Litter in bin",
      "Unicode": "U+1F6AE"
    },
    {
      "Emoji": "🚰",
      "Meaning": "Portable water",
      "Unicode": "U+1F6B0"
    },
    {
      "Emoji": "♿",
      "Meaning": "Wheelchair symbol",
      "Unicode": "U+267F"
    },
    {
      "Emoji": "🚹",
      "Meaning": "Men's room symbol",
      "Unicode": "U+1F6B9"
    },
    {
      "Emoji": "🚺",
      "Meaning": "Women's room symbol",
      "Unicode": "U+1F6BA"
    },
    {
      "Emoji": "🚻",
      "Meaning": "Restroom symbol",
      "Unicode": "U+1F6BB"
    },
    {
      "Emoji": "🚼",
      "Meaning": "Baby symbol",
      "Unicode": "U+1F6BC"
    },
    {
      "Emoji": "🚾",
      "Meaning": "Water closet",
      "Unicode": "U+1F6BE"
    },
    {
      "Emoji": "🛂",
      "Meaning": "Passport control",
      "Unicode": "U+1F6C2"
    },
    {
      "Emoji": "🛂",
      "Meaning": "Customs",
      "Unicode": "U+1F6C3"
    },
    {
      "Emoji": "🛄",
      "Meaning": "Baggage claim",
      "Unicode": "U+1F6C4"
    },
    {
      "Emoji": "🛅",
      "Meaning": "Left laugage",
      "Unicode": "U+1F6C5"
    },
    {
      "Emoji": "⚠",
      "Meaning": "Warning",
      "Unicode": "U+26A0"
    },
    {
      "Emoji": "🚸",
      "Meaning": "Children crossing",
      "Unicode": "U+1F6B8"
    },
    {
      "Emoji": "⛔",
      "Meaning": "No entry",
      "Unicode": "U+26D4"
    },
    {
      "Emoji": "🚫",
      "Meaning": "Prohibited",
      "Unicode": "U+1F6AB"
    },
    {
      "Emoji": "🚳",
      "Meaning": "No bicycles",
      "Unicode": "U+1F6B3"
    },
    {
      "Emoji": "🚭",
      "Meaning": "No smoking",
      "Unicode": "U+1F6AD"
    },
    {
      "Emoji": "🚯",
      "Meaning": "No littering",
      "Unicode": "U+1F6AF"
    },
    {
      "Emoji": "🚱",
      "Meaning": "Non-portable water",
      "Unicode": "U+1F6B1"
    },
    {
      "Emoji": "🚷",
      "Meaning": "No pedestrians",
      "Unicode": "U+1F6B7"
    },
    {
      "Emoji": "📵",
      "Meaning": "No mobile phones",
      "Unicode": "U+1F4F5"
    },
    {
      "Emoji": "🔞",
      "Meaning": "No one under 18",
      "Unicode": "U+1F51E"
    },
    {
      "Emoji": "☢",
      "Meaning": "Radioactive",
      "Unicode": "U+2622"
    },
    {
      "Emoji": "☣",
      "Meaning": "Biohazard",
      "Unicode": "U+2623"
    },
    {
      "Emoji": "⬆",
      "Meaning": "Up Arrow",
      "Unicode": "U+2B06"
    },
    {
      "Emoji": "↗",
      "Meaning": "Up-right arrow",
      "Unicode": "U+2197"
    },
    {
      "Emoji": "➡",
      "Meaning": "Right arrow",
      "Unicode": "U+27A1"
    },
    {
      "Emoji": "↘",
      "Meaning": "Down-right arrow",
      "Unicode": "U+2198"
    },
    {
      "Emoji": "⬇",
      "Meaning": "Down arrow",
      "Unicode": "U+2B07"
    },
    {
      "Emoji": "↙",
      "Meaning": "Down-left arrow",
      "Unicode": "U+2199"
    },
    {
      "Emoji": "⬅",
      "Meaning": "Left arrow",
      "Unicode": "U+2B05"
    },
    {
      "Emoji": "↖",
      "Meaning": "Up-left arrow",
      "Unicode": "U+2196"
    },
    {
      "Emoji": "↕",
      "Meaning": "Up-down arrow",
      "Unicode": "U+2195"
    },
    {
      "Emoji": "↔",
      "Meaning": "Left arrow",
      "Unicode": "U+2194"
    },
    {
      "Emoji": "↩",
      "Meaning": "Right arrow curving left",
      "Unicode": "U+21A9"
    },
    {
      "Emoji": "↪",
      "Meaning": "Left arrow curving right",
      "Unicode": "U+21AA"
    },
    {
      "Emoji": "⤴",
      "Meaning": "Right arrow curving up",
      "Unicode": "U+2934"
    },
    {
      "Emoji": "⤵",
      "Meaning": "Right arrow curving down",
      "Unicode": "U+2935"
    },
    {
      "Emoji": "🔃",
      "Meaning": "Clockwise vertical arrow",
      "Unicode": "U+1F503"
    },
    {
      "Emoji": "🔄",
      "Meaning": "Counterclockwise arrows button",
      "Unicode": "U+1F504"
    },
    {
      "Emoji": "🔙",
      "Meaning": "Back arrow",
      "Unicode": "U+1F519"
    },
    {
      "Emoji": "🔚",
      "Meaning": "End arrow",
      "Unicode": "U+1F51A"
    },
    {
      "Emoji": "🔛",
      "Meaning": "On arrow",
      "Unicode": "U+1F51B"
    },
    {
      "Emoji": "🔜",
      "Meaning": "Soon arrow",
      "Unicode": "U+1F51C"
    },
    {
      "Emoji": "🔝",
      "Meaning": "Top arrow",
      "Unicode": "U+1F51D"
    },
    {
      "Emoji": "🛐",
      "Meaning": "Place of worship",
      "Unicode": "U+1F6D0"
    },
    {
      "Emoji": "⚛",
      "Meaning": "Atom symbol",
      "Unicode": "U+269B"
    },
    {
      "Emoji": "🕉",
      "Meaning": "OM",
      "Unicode": "U+1F549"
    },
    {
      "Emoji": "✡",
      "Meaning": "Star of David",
      "Unicode": "U+2721"
    },
    {
      "Emoji": "☸",
      "Meaning": "Wheel of Dharma",
      "Unicode": "U+2638"
    },
    {
      "Emoji": "☯",
      "Meaning": "Yin yang",
      "Unicode": "U+262F"
    },
    {
      "Emoji": "✝",
      "Meaning": "Latin cross",
      "Unicode": "U+271D"
    },
    {
      "Emoji": "☦",
      "Meaning": "ORthodox cross",
      "Unicode": "U+2626"
    },
    {
      "Emoji": "☪",
      "Meaning": "Star and cresent moon",
      "Unicode": "U+262A"
    },
    {
      "Emoji": "☮",
      "Meaning": "Peace",
      "Unicode": "U+262E"
    },
    {
      "Emoji": "🕎",
      "Meaning": "Menorah",
      "Unicode": "U+1F54E"
    },
    {
      "Emoji": "🔯",
      "Meaning": "Six-pointed star",
      "Unicode": "U+1F52F"
    },
    {
      "Emoji": "♈",
      "Meaning": "Aries",
      "Unicode": "U+2648"
    },
    {
      "Emoji": "♉",
      "Meaning": "Taurus",
      "Unicode": "U+2649"
    },
    {
      "Emoji": "♊",
      "Meaning": "Gemini",
      "Unicode": "U+264A"
    },
    {
      "Emoji": "♋",
      "Meaning": "Cancer",
      "Unicode": "U+264B"
    },
    {
      "Emoji": "♌",
      "Meaning": "Leo",
      "Unicode": "U+264C"
    },
    {
      "Emoji": "♍",
      "Meaning": "Virgo",
      "Unicode": "U+264D"
    },
    {
      "Emoji": "♎",
      "Meaning": "Libra",
      "Unicode": "U+264E"
    },
    {
      "Emoji": "♏",
      "Meaning": "Scorpio",
      "Unicode": "U+264F"
    },
    {
      "Emoji": "♐",
      "Meaning": "Sagittarius",
      "Unicode": "U+2650"
    },
    {
      "Emoji": "♑",
      "Meaning": "Capricon",
      "Unicode": "U+2651"
    },
    {
      "Emoji": "♒",
      "Meaning": "Acquarius",
      "Unicode": "U+2652"
    },
    {
      "Emoji": "♓",
      "Meaning": "Pisces",
      "Unicode": "U+2653"
    },
    {
      "Emoji": "⛎",
      "Meaning": "Ophiucus",
      "Unicode": "U+26CE"
    },
    {
      "Emoji": "🔀",
      "Meaning": "Shuffle tracks",
      "Unicode": "U+1F500"
    },
    {
      "Emoji": "🔁",
      "Meaning": "Repeat all",
      "Unicode": "U+1F501"
    },
    {
      "Emoji": "🔂",
      "Meaning": "Repeat one",
      "Unicode": "U+1F502"
    },
    {
      "Emoji": "▶",
      "Meaning": "Play",
      "Unicode": "U+25B6"
    },
    {
      "Emoji": "⏸",
      "Meaning": "Pause",
      "Unicode": "U+23F8"
    },
    {
      "Emoji": "⏩",
      "Meaning": "Fast-forward",
      "Unicode": "U+23E9"
    },
    {
      "Emoji": "⏭",
      "Meaning": "Next track",
      "Unicode": "U+23ED"
    },
    {
      "Emoji": "⏯",
      "Meaning": "Play or pause",
      "Unicode": "U+23EF"
    },
    {
      "Emoji": "◀",
      "Meaning": "Reverse",
      "Unicode": "U+25C0"
    },
    {
      "Emoji": "⏪",
      "Meaning": "Fast-reverse",
      "Unicode": "U+23EA"
    },
    {
      "Emoji": "⏮",
      "Meaning": "Previous track",
      "Unicode": "U+23EE"
    },
    {
      "Emoji": "🔼",
      "Meaning": "Upwards",
      "Unicode": "U+1F53C"
    },
    {
      "Emoji": "⏫",
      "Meaning": "Fst-up",
      "Unicode": "U+23EB"
    },
    {
      "Emoji": "🔽",
      "Meaning": "Downwards",
      "Unicode": "U+1F53D"
    },
    {
      "Emoji": "⏬",
      "Meaning": "Fast down",
      "Unicode": "U+23EC"
    },
    {
      "Emoji": "⏹",
      "Meaning": "Stop",
      "Unicode": "U+23F9"
    },
    {
      "Emoji": "⏺",
      "Meaning": "Record",
      "Unicode": "U+23FA"
    },
    {
      "Emoji": "⏏",
      "Meaning": "Eject",
      "Unicode": "U+23CF"
    },
    {
      "Emoji": "🎦",
      "Meaning": "Cinema",
      "Unicode": "U+1F3A6"
    },
    {
      "Emoji": "🔅",
      "Meaning": "Dim",
      "Unicode": "U+1F505"
    },
    {
      "Emoji": "🔆",
      "Meaning": "Bright",
      "Unicode": "U+1F506"
    },
    {
      "Emoji": "📶",
      "Meaning": "Network antenna bars",
      "Unicode": "U+1F4F6"
    },
    {
      "Emoji": "📳",
      "Meaning": "Vibration mode",
      "Unicode": "U+1F4F3"
    },
    {
      "Emoji": "📴",
      "Meaning": "Mobile phone off",
      "Unicode": "U+1F4F4"
    },
    {
      "Emoji": "♀",
      "Meaning": "Female",
      "Unicode": "U+2640"
    },
    {
      "Emoji": "♂",
      "Meaning": "Male",
      "Unicode": "U+2642"
    },
    {
      "Emoji": "⚧",
      "Meaning": "Transgender",
      "Unicode": "U+26A7"
    },
    {
      "Emoji": "✖",
      "Meaning": "Times",
      "Unicode": "U+2716"
    },
    {
      "Emoji": "➕",
      "Meaning": "Plus",
      "Unicode": "U+2795"
    },
    {
      "Emoji": "➖",
      "Meaning": "Minus",
      "Unicode": "U+2796"
    },
    {
      "Emoji": "➗",
      "Meaning": "Divide",
      "Unicode": "U+2797"
    },
    {
      "Emoji": "🟰",
      "Meaning": "Equals",
      "Unicode": "U+1F7F0"
    },
    {
      "Emoji": "♾",
      "Meaning": "Infinity",
      "Unicode": "U+267E"
    },
    {
      "Emoji": "‼",
      "Meaning": "Double exclamation",
      "Unicode": "U+203C"
    },
    {
      "Emoji": "⁉",
      "Meaning": "Exclamation and question mark",
      "Unicode": "U+2049"
    },
    {
      "Emoji": "❓",
      "Meaning": "Red question mark",
      "Unicode": "U+2753"
    },
    {
      "Emoji": "❔",
      "Meaning": "White question mark",
      "Unicode": "U+2754"
    },
    {
      "Emoji": "❗",
      "Meaning": "Red exclamation mark",
      "Unicode": "U+2757"
    },
    {
      "Emoji": "❕",
      "Meaning": "White exclamation mark",
      "Unicode": "U+2755"
    },
    {
      "Emoji": "〰",
      "Meaning": "Wavy dash",
      "Unicode": "U+3030"
    },
    {
      "Emoji": "💱",
      "Meaning": "Currency exchange",
      "Unicode": "U+1F4B1"
    },
    {
      "Emoji": "💲",
      "Meaning": "Heavy green dollar sign",
      "Unicode": "U+1F4B2"
    },
    {
      "Emoji": "⚕",
      "Meaning": "Medical symbol",
      "Unicode": "U+2695"
    },
    {
      "Emoji": "♻",
      "Meaning": "Recycling symbol",
      "Unicode": "U+267B"
    },
    {
      "Emoji": "⚜",
      "Meaning": "Fleur-de-lis",
      "Unicode": "U+269C"
    },
    {
      "Emoji": "🔱",
      "Meaning": "Trident",
      "Unicode": "U+1F531"
    },
    {
      "Emoji": "📛",
      "Meaning": "Name badge",
      "Unicode": "U+1F4DB"
    },
    {
      "Emoji": "🔰",
      "Meaning": "Japanese symbol for beginner",
      "Unicode": "U+1F530"
    },
    {
      "Emoji": "⭕",
      "Meaning": "Hollow red circle",
      "Unicode": "U+2B55"
    },
    {
      "Emoji": "✅",
      "Meaning": "Green box with checkmark",
      "Unicode": "U+2705"
    },
    {
      "Emoji": "☑",
      "Meaning": "Blue box with checkmark",
      "Unicode": "U+2611"
    },
    {
      "Emoji": "✔",
      "Meaning": "Checkmark",
      "Unicode": "U+2714"
    },
    {
      "Emoji": "❌",
      "Meaning": "Crossmark",
      "Unicode": "U+274C"
    },
    {
      "Emoji": "❎",
      "Meaning": "Green crossmark",
      "Unicode": "U+274E"
    },
    {
      "Emoji": "➰",
      "Meaning": "Curly loop",
      "Unicode": "U+27B0"
    },
    {
      "Emoji": "➿",
      "Meaning": "Double curly loop",
      "Unicode": "U+27BF"
    },
    {
      "Emoji": "〽",
      "Meaning": "PArt alternation mark",
      "Unicode": "U+303D"
    },
    {
      "Emoji": "✳",
      "Meaning": "Eight-spoked asterik",
      "Unicode": "U+2733"
    },
    {
      "Emoji": "✴",
      "Meaning": "Eight-pointed star",
      "Unicode": "U+2734"
    },
    {
      "Emoji": "❇",
      "Meaning": "Sparkle",
      "Unicode": "U+2747"
    },
    {
      "Emoji": "©",
      "Meaning": "Copyright symbol",
      "Unicode": "U+00A9"
    },
    {
      "Emoji": "®",
      "Meaning": "Registered",
      "Unicode": "U+00AE"
    },
    {
      "Emoji": "™",
      "Meaning": "Trademark",
      "Unicode": "U+2122"
    },
    {
      "Emoji": "#️⃣",
      "Meaning": "# Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "*️⃣",
      "Meaning": "* Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "0️⃣",
      "Meaning": "0 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "1️⃣",
      "Meaning": "1 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "2️⃣",
      "Meaning": "2 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "3️⃣",
      "Meaning": "3 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "4️⃣",
      "Meaning": "4 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "5️⃣",
      "Meaning": "5 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "6️⃣",
      "Meaning": "6 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "7️⃣",
      "Meaning": "7 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "8️⃣",
      "Meaning": "8 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "9️⃣",
      "Meaning": "9 Keycap",
      "Unicode": "U+20E3"
    },
    {
      "Emoji": "🔟",
      "Meaning": "10 Keycap",
      "Unicode": "U+1F51F"
    },
    {
      "Emoji": "🔠",
      "Meaning": "Input Latin uppercase",
      "Unicode": "U+1F520"
    },
    {
      "Emoji": "🔡",
      "Meaning": "Input Latin lowercase",
      "Unicode": "U+1F521"
    },
    {
      "Emoji": "🔢",
      "Meaning": "Input numbers",
      "Unicode": "U+1F522"
    },
    {
      "Emoji": "🔣",
      "Meaning": "Input symbols",
      "Unicode": "U+1F523"
    },
    {
      "Emoji": "🔤",
      "Meaning": "Input Latin letters",
      "Unicode": "U+1F524"
    },
    {
      "Emoji": "🅰",
      "Meaning": "A blood type",
      "Unicode": "U+1F170"
    },
    {
      "Emoji": "🆎",
      "Meaning": "AB blood type",
      "Unicode": "U+1F18E"
    },
    {
      "Emoji": "🅱",
      "Meaning": "B blood type",
      "Unicode": "U+1F171"
    },
    {
      "Emoji": "🅾",
      "Meaning": "O blood type",
      "Unicode": "U+1F17E"
    },
    {
      "Emoji": "🆑",
      "Meaning": "CL button",
      "Unicode": "U+1F191"
    },
    {
      "Emoji": "🆒",
      "Meaning": "Cool button",
      "Unicode": "U+1F192"
    },
    {
      "Emoji": "🆓",
      "Meaning": "Free button",
      "Unicode": "U+1F193"
    },
    {
      "Emoji": "ℹ",
      "Meaning": "Info button",
      "Unicode": "U+2139"
    },
    {
      "Emoji": "🆔",
      "Meaning": "ID button",
      "Unicode": "U+1F194"
    },
    {
      "Emoji": "Ⓜ",
      "Meaning": "Circled M",
      "Unicode": "U+24C2"
    },
    {
      "Emoji": "🆕",
      "Meaning": "New button",
      "Unicode": "U+1F195"
    },
    {
      "Emoji": "🆖",
      "Meaning": "NG button",
      "Unicode": "U+1F196"
    },
    {
      "Emoji": "🆗",
      "Meaning": "OK button",
      "Unicode": "U+1F197"
    },
    {
      "Emoji": "🅿",
      "Meaning": "P button",
      "Unicode": "U+1F17F"
    },
    {
      "Emoji": "🆘",
      "Meaning": "SOS button",
      "Unicode": "U+1F198"
    },
    {
      "Emoji": "🆙",
      "Meaning": "UP! button",
      "Unicode": "U+1F199"
    },
    {
      "Emoji": "🆚",
      "Meaning": "VS Button",
      "Unicode": "U+1F19A"
    },
    {
      "Emoji": "🈁",
      "Meaning": "Japanese \"here\" button",
      "Unicode": "U+1F201"
    },
    {
      "Emoji": "🈂",
      "Meaning": "Japanese \"service charge\" button",
      "Unicode": "U+1F202"
    },
    {
      "Emoji": "🈷",
      "Meaning": "Japanese \"monthly amount\" button",
      "Unicode": "U+1F237"
    },
    {
      "Emoji": "🈶",
      "Meaning": "Japanese \"not free of charge\" button",
      "Unicode": "U+1F236"
    },
    {
      "Emoji": "🈯",
      "Meaning": "Japanese \"reserved\" button",
      "Unicode": "U+1F22F"
    },
    {
      "Emoji": "🉐",
      "Meaning": "Japanese \"bargain\" button",
      "Unicode": "U+1F250"
    },
    {
      "Emoji": "🈹",
      "Meaning": "Japanese \"discount\" button",
      "Unicode": "U+1F239"
    },
    {
      "Emoji": "🈚",
      "Meaning": "Japanese \"free of charge\" button",
      "Unicode": "U+1F21A"
    },
    {
      "Emoji": "🈲",
      "Meaning": "Japanese \"prohibited\" button",
      "Unicode": "U+1F232"
    },
    {
      "Emoji": "🉑",
      "Meaning": "Japanese \"acceptable\" button",
      "Unicode": "U+1F251"
    },
    {
      "Emoji": "🈸",
      "Meaning": "Japanese \"application\" button",
      "Unicode": "U+1F238"
    },
    {
      "Emoji": "🈴",
      "Meaning": "Japanese \"passing grade\" button",
      "Unicode": "U+1F234"
    },
    {
      "Emoji": "🈳",
      "Meaning": "Japanese \"vacancy\" button",
      "Unicode": "U+1F233"
    },
    {
      "Emoji": "㊗",
      "Meaning": "Japanese \"congratulations\" button",
      "Unicode": "U+3297"
    },
    {
      "Emoji": "㊙",
      "Meaning": "Japanese \"secret\" button",
      "Unicode": "U+3299"
    },
    {
      "Emoji": "🈺",
      "Meaning": "Japanese \"open for business\" button",
      "Unicode": "U+1F23A"
    },
    {
      "Emoji": "🈵",
      "Meaning": "Japanese \"no vacancy\" button",
      "Unicode": "U+1F235"
    },
    {
      "Emoji": "🔴",
      "Meaning": "Red circle",
      "Unicode": "U+1F534"
    },
    {
      "Emoji": "🟠",
      "Meaning": "Orange circle",
      "Unicode": "U+1F7E0"
    },
    {
      "Emoji": "🟡",
      "Meaning": "Yellow circle",
      "Unicode": "U+1F7E1"
    },
    {
      "Emoji": "🟢",
      "Meaning": "Green circle",
      "Unicode": "U+1F7E2"
    },
    {
      "Emoji": "🔵",
      "Meaning": "Blue circle",
      "Unicode": "U+1F535"
    },
    {
      "Emoji": "🟣",
      "Meaning": "Purple circle",
      "Unicode": "U+1F7E3"
    },
    {
      "Emoji": "🟤",
      "Meaning": "Brown circle",
      "Unicode": "U+1F7E4"
    },
    {
      "Emoji": "⚫",
      "Meaning": "Black circle",
      "Unicode": "U+26AB"
    },
    {
      "Emoji": "⚪",
      "Meaning": "White circle",
      "Unicode": "U+26AA"
    },
    {
      "Emoji": "🟥",
      "Meaning": "Red square",
      "Unicode": "U+1F7E5"
    },
    {
      "Emoji": "🟧",
      "Meaning": "Orange square",
      "Unicode": "U+1F7E5"
    },
    {
      "Emoji": "🟨",
      "Meaning": "Yellow square",
      "Unicode": "U+1F7E8"
    },
    {
      "Emoji": "🟩",
      "Meaning": "Green square",
      "Unicode": "U+1F7E9"
    },
    {
      "Emoji": "🟦",
      "Meaning": "Blue square",
      "Unicode": "U+1F7E6"
    },
    {
      "Emoji": "🟪",
      "Meaning": "Purple square",
      "Unicode": "U+1F7EA"
    },
    {
      "Emoji": "🟫",
      "Meaning": "Brown square",
      "Unicode": "U+1F7EB"
    },
    {
      "Emoji": "⬛",
      "Meaning": "Black square",
      "Unicode": "U+2B1B"
    },
    {
      "Emoji": "⬜",
      "Meaning": "White square",
      "Unicode": "U+2B1C"
    },
    {
      "Emoji": "🔶",
      "Meaning": "Large orange diamond",
      "Unicode": "U+1F536"
    },
    {
      "Emoji": "🔷",
      "Meaning": "Large blue diamond",
      "Unicode": "U+1F537"
    },
    {
      "Emoji": "🔸",
      "Meaning": "Small orange diamond",
      "Unicode": "U+1F538"
    },
    {
      "Emoji": "🔹",
      "Meaning": "Small blue diamond",
      "Unicode": "U+1F539"
    },
    {
      "Emoji": "🔺",
      "Meaning": "Red triangle pointed up",
      "Unicode": "U+1F53A"
    },
    {
      "Emoji": "🔻",
      "Meaning": "Red triangle pointed down",
      "Unicode": "U+1F53B"
    },
    {
      "Emoji": "💠",
      "Meaning": "Diamond with a dot",
      "Unicode": "U+1F4A0"
    },
    {
      "Emoji": "🔘",
      "Meaning": "Radio button",
      "Unicode": "U+1F518"
    },
    {
      "Emoji": "🔳",
      "Meaning": "White square button",
      "Unicode": "U+1F533"
    },
    {
      "Emoji": "🔲",
      "Meaning": "Black square button",
      "Unicode": "U+1F532"
    },
   ]

}
