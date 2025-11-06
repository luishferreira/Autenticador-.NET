using MediatR;

namespace Autenticador.Application.Common.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest;

public interface IQuery<out TResponse> : IRequest<TResponse>;