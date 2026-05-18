#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main() {
    srand(time(NULL));

    int darts_to_throw = 1000000;
    int darts_within_dartboard = 0;

    for (int i = 0; i < darts_to_throw; i++) {
        double rand_x = ((double) rand() / RAND_MAX) * 2.0 - 1.0;
        double rand_y = ((double) rand() / RAND_MAX) * 2.0 - 1.0;

        double dist_squared = rand_x * rand_x + rand_y * rand_y;

        if (dist_squared <= 1.0) {
            darts_within_dartboard++;
        }
    }

    double pi_est = 4.0 * (double)darts_within_dartboard / darts_to_throw;

    printf("Darts thrown: %d\n", darts_to_throw);
    printf("Darts inside circle: %d\n", darts_within_dartboard);
    printf("The estimated value of pi is %f\n", pi_est);

    return 0;
}
