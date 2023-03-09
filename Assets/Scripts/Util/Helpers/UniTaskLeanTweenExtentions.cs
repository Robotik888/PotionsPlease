using Cysharp.Threading.Tasks;

public static class UniTaskLeanTweenExtentions
{
    public static async UniTask ToUniTaskAsync(this LTDescr source)
    {
        var tcs = new UniTaskCompletionSource();
        source.setOnComplete(() => tcs.TrySetResult());
        await tcs.Task;
    }
}
