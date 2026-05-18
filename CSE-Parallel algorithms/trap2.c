#include <stdio.h>
#include <stdlib.h>
#include <omp.h>

double result = 0.0;
int thread_count;

double f(double x) {
    return x * x;
}

double Trap(double a, double b, int n) {
    double h = (b - a) / n;
    double local_a, local_b;
    int local_n;
    int my_rank = omp_get_thread_num();

    local_n = n / thread_count;
    local_a = a + my_rank * local_n * h;
    local_b = local_a + local_n * h;

    double my_result = (f(local_a) + f(local_b)) / 2.0;

    for (int i = 1; i < local_n; i++) {
        double x = local_a + i * h;
        my_result += f(x);
    }

    my_result = my_result * h;
    return my_result;
}

int main(int argc, char* argv[]) {
    double a = 0.0;
    double b = 10.0;
    int n = 1000000;

    thread_count = 10;

    #pragma omp parallel num_threads(thread_count)
    {
        double my_result = Trap(a, b, n);

        #pragma omp critical
        result += my_result;
    }

    printf("With n = %d trapezoids, our estimate\n", n);
    printf("of the integral from %f to %f = %.15f\n", a, b, result);

    return 0;
}
