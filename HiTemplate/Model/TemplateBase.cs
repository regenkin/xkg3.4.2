using System;
using System.IO;

namespace HiTemplate.Model
{
	public abstract class TemplateBase
	{
		private TextWriter writer;

		public string Layout
		{
			get;
			set;
		}

		public Func<string> RenderBody
		{
			get;
			set;
		}

		public string Path
		{
			get;
			internal set;
		}

		public string Result
		{
			get
			{
				return this.Writer.ToString();
			}
		}

		public TextWriter Writer
		{
			get
			{
				if (this.writer == null)
				{
					this.writer = new StringWriter();
				}
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		public void Clear()
		{
			this.Writer.Flush();
		}

		public virtual void Execute()
		{
		}

		public void Write(object @object)
		{
			if (@object != null)
			{
				this.Writer.Write(@object);
			}
		}

		public void WriteLiteral(string @string)
		{
			if (@string != null)
			{
				this.Writer.Write(@string);
			}
		}

		public static void WriteLiteralTo(TextWriter writer, string literal)
		{
			if (literal != null)
			{
				writer.Write(literal);
			}
		}

		public static void WriteTo(TextWriter writer, object obj)
		{
			if (obj != null)
			{
				writer.Write(obj);
			}
		}
	}
	public abstract class TemplateBase<T> : TemplateBase
	{
		public T Model
		{
			get;
			set;
		}
	}
}
