export class ScrollEventListener {
  /** Just one handler per funciton */
  onIncress: () => void = () => { };
  onDecress: () => void = () => { };
  onChange: (fromHeight: number, toHeight: number) => void = () => { };
  onUserScroll: (distanceFromTop: number, TotalHeigh: number) => void =
    () => { };

  constructor(private NativeElementFactory: () => any) {
    this.interval = setInterval(() => this.ScrollEventListener(), 200);
    this.ScrollEventListener();
  }

  public DoOnNextHeightIncress(ToDo: () => void) {
    this.onIncress = () => {
      ToDo();
      this.onIncress = () => { };
    };
  }

  /** Call this methods on "ngOnDestroy" */
  public Dispose() {
    clearInterval(this.interval);
  }
  private interval: any;

  private lastHeight = 0;
  private lastScrollTop = 0;
  private ScrollEventListener() {
    var element = this.NativeElementFactory();
    if (element === undefined || element === null) return;
    var scrollHeight = element.scrollHeight;
    if (scrollHeight !== undefined && scrollHeight !== this.lastHeight) {
      setTimeout(() => {
        if (scrollHeight > this.lastHeight) this.onIncress();
        else if (scrollHeight < this.lastHeight) this.onDecress();
        this.onChange(this.lastHeight, scrollHeight);
        this.lastHeight = scrollHeight;
      }, 20);
    }
    var scrollTop = element.scrollTop;
    if (scrollTop !== this.lastScrollTop) {
      this.onUserScroll(scrollTop, scrollHeight);
      this.lastScrollTop = scrollTop;
    }
  }
}
