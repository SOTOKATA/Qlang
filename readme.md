# Qlang (Quick Language)

Qlang - modern programming language, compiler and runtime written on c#. Syntax similar to TypeScript and C#

Example of "Hello, World!":

```ql
import "$lib/standard"

function main(): {
  std::console.println("Hello, World!");
}
```

# How to use?

To create new project use command-line interface "ql" and type:

`ql new [project_name]`

After, to compile and run project type:

`ql run`

For information about other commands type:

`ql help`
