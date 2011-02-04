namespace HelloWorldApp.BusinessObjects
{
    public class GenericObject<T>
    {
        public GenericObject()
        {
        }

        public GenericObject(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}