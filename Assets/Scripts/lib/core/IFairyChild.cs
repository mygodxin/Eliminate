using FairyGUI;

namespace mm
{
    interface IFairyChild
    {
        void setRoot(GComponent view);
        GComponent getComp(string path);
        GButton getButton(string path);
        GLabel getLabel(string path);
        GProgressBar getProgressBar(string path);
        GTextField getTextField(string path);
        GRichTextField getRichTextField(string path);
        GTextInput getTextInput(string path);
        GLoader getLoader(string path);
        GList getList(string path);
        GGraph getGraph(string path);
        GGroup getGroup(string path);
        GSlider getSlider(string path);
        GComboBox getComboBox(string path);
        GImage getImage(string path);
        GMovieClip getMovieClip(string path);
        Controller getController(string path);
        Transition getTransition(string path);
        Window getWindow(string path);
    }
}