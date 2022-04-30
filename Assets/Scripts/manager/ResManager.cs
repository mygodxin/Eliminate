using mm;
public class ResManager
{
    public static readonly ResManager inst = new ResManager();
    private SourcePreLoader fairyPreLoader;
    public FairyLoader commonFL;
    public FairyLoader startFL;
    public FairyLoader gameFL;
    ResManager()
    {
        this.commonFL = new FairyLoader("common");
        this.startFL = new FairyLoader("start");
        this.gameFL = new FairyLoader("game");
    }

    public void Preload()
    {
        //首包
        // this.fairyPreLoader = new k7.SourcePreLoader();
        // this.fairyPreLoader.addSource(this.loadingFL);
        // this.fairyPreLoader.preload();
        this.fairyPreLoader = new SourcePreLoader();
        this.fairyPreLoader.addSource(new ASourceLoader[]{
            this.commonFL,this.startFL,this.gameFL
        });
        this.fairyPreLoader.preload();
    }
}