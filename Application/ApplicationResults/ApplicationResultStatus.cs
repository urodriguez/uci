namespace Application.ApplicationResults
{
    public enum ApplicationResultStatus
    {
        Ok,
        BadRequest,//RFC 7231 => the server cannot or will not process the request due to something that is perceived to be a client error
        Unauthenticated,
        Unauthorized,
        NotFound,
        UnsupportedMediaType,
        InternalServerError
    }
}