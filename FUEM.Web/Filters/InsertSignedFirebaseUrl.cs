using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FUEM.Web.Filters
{
    public class InsertSignedFirebaseUrl : IAsyncResultFilter
    {
        //private readonly FirebaseStorageService _firebaseStorage;

        public InsertSignedFirebaseUrl()
        {
            //_firebaseStorage = firebaseStorage;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result && result.Value != null)
            {
               ProcessObject(result.Value);
            }

            await next();
        }

        private void ProcessObject(object value)
        {
            if (value is List<object>)
            {
                Console.WriteLine(value);
            }
        }
    }
}
