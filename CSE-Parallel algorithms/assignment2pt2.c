#include <stdio.h>
#include <stdlib.h>
#include <omp.h>

double f(double x) {
    return x * x;
}

int main(int argc, char* argv[]) {
    int thread_count = strtol(argv[1], NULL, 10);

    double a = 0.0;
    double b = 1.0;
    int n = 20;

    double h = (b - a) / n;
    double approx;
    double x;
    int my_rank;

    approx = (f(a) + f(b)) / 2.0;

    #pragma omp parallel for num_threads(thread_count) reduction(+:approx) private(x, my_rank)
    for (int i = 1; i <= n - 1; i++) {
        my_rank = omp_get_thread_num();
        x = a + i * h;
        printf("Thread %d of %d has calculated x: %f\n", my_rank, thread_count, x);
        approx += f(x);
    }

    approx = approx * h;

    printf("\nWith n = %d trapezoids,\n", n);
    printf("Estimated area from %f to %f = %.15f\n", a, b, approx);

    return 0;
}
