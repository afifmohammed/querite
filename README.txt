Queries are an interesting concern with every application. One that makes or breaks it. Querying support in applications come in all shapes and forms. With Querite the intent is to help keep queries to the most basic i.e. pocos. The support for defining each query as a poco and executing it by calling a query handler.

Suppose you have a class in your application called Folder that can tell you the name of all files it has.
	public class Folder {
		public IEnumerable<string> Files {get;}
	}
	
you wish to query this folder 

	// for all files whose names are of a certain length.
	public class FileNamesByLengthQuery : IAmQuery<IEnumerable<string>, Folder>
	{
		public int Length {get;set;
		public IEnumerable<string> Apply(Folder source)
		{
			return source.Where(x => x.Length == this.Length);
		}
	}
	
Or
	// count all files whose names are of a certain length.
	public class CountFileNamesByLengthQuery : IAmQuery<int, Folder>
	{
		public int Length {get;set;
		public int Apply(Folder source)
		{
			return source.Count(x => x.Length == this.Length);
		}
	}
	
And simple execute the query as follows.

	private IQuery<TSource> Query<TSource>()
	{
		// many different ways to do this 
		// without having to reference the ninject kernel.
		// only done so for brevity.
		return Kernel.Get<IQuery<TSource>>();
	}
	
	var names = Query<Folder>.Execute(new FileNamesByLengthQuery {Length = 3});
	var total = Query<Folder>.Execute(new CountFileNamesByLengthQuery { Length = 4});
	

Querite gives you a clean unified pattern for defining and executing queries, irrespective of whether they are against a class named 'Folder' that can tell us all the file names, or against an ISession, or against an IMongoSession or against a DbContext, or against 'insert your favourite data source'.

The biggest benefit with querying in this manner is it does not voilate the single responsibility principle. 

The IQuery<TSource> implementation in Querite has the responsibility of executing the query, where as the query classes that you write, have the responsibility of defining query criteria and specifying how its applied. So there's a seperation between the 'how its done' and 'who does it'. 

Also since query classes are represented as pocos which can be newed up with properties and executed, a pattern is provided that discourages you from adding dependencies to your query class.