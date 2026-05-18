#include <stdio.h>
#include <string.h>

/* creation of the char and the ids for what we need to create our grammar so S A Aprime B*/
char input[50];
char *ip;

int S();
int A();
int Aprime();
int B();

/* creation of the recursion between S -> aAB with output*/

int S()
{
    printf("%s\tS -> aAB\n", ip);

    if (*ip == 'a')
    {
        ip++;

        if (A())
        {
            if (B())
            {
                return 1;
            }
        }
    }

    return 0;
}

/* creation of the recursion between A -> bA*/

int A()
{
    printf("%s\tA -> bA'\n", ip);

    if (*ip == 'b')
    {
        ip++;

        if (Aprime())
        {
            return 1;
        }
    }

    return 0;
}

/* output for Aprime A -> bcA*/

int Aprime()
{
    if (*ip == 'b')
    {
        printf("%s\tA' -> bcA'\n", ip);

        ip++;

        if (*ip == 'c')
        {
            ip++;

            if (Aprime())
            {
                return 1;
            }
        }

        return 0;
    }
    else
    {
        printf("%s\tA' -> epsilon\n", ip);
        return 1;
    }
}

/* creation of recursion of B -> d*/

int B()
{
    printf("%s\tB -> d\n", ip);

    if (*ip == 'd')
    {
        ip++;
        return 1;
    }

    return 0;
}

/* Main part of the program where user inputs the string of characters that create the output wheather its valid or invalid based on our recursive program*/

int main()
{
    printf("Enter the string: ");
    scanf("%49s", input);

    ip = input;

    printf("\nInput\tAction\n");
    printf("-----------------------------------\n");

    if (S() && *ip == '\0')
    {
        printf("-----------------------------------\n");
        printf("String is successfully parsed\n");
    }
    else
    {
        printf("-----------------------------------\n");
        printf("Error in parsing string\n");
    }

    return 0;
}
