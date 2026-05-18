#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <omp.h>

void printArraySample(int arr[], int n, int count) {
    int i;

    if (n <= 2 * count) {
        for (i = 0; i < n; i++) {
            printf("%d ", arr[i]);
        }
        printf("\n");
        return;
    }

    for (i = 0; i < count; i++) {
        printf("%d ", arr[i]);
    }

    printf("... ");

    for (i = n - count; i < n; i++) {
        printf("%d ", arr[i]);
    }

    printf("\n");
}

void fillRandomArray(int arr[], int n) {
    int i;
    for (i = 0; i < n; i++) {
        arr[i] = rand() % 100000;
    }
}

int isSorted(int arr[], int n) {
    int i;
    for (i = 0; i < n - 1; i++) {
        if (arr[i] > arr[i + 1]) {
            return 0;
        }
    }
    return 1;
}

void oddEvenSortSingle(int arr[], int n) {
    int p, i, temp;

    for (p = 0; p < n; p++) {
        if (p % 2 == 0) {
            for (i = 0; i < n - 1; i += 2) {
                if (arr[i] > arr[i + 1]) {
                    temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }
            }
        } else {
            for (i = 1; i < n - 1; i += 2) {
                if (arr[i] > arr[i + 1]) {
                    temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }
            }
        }
    }
}

int main() {
    int n = 10000;
    int *arr = (int *)malloc(n * sizeof(int));
    double start, end;

    if (arr == NULL) {
        printf("Memory allocation failed.\n");
        return 1;
    }

    srand(time(NULL));
    fillRandomArray(arr, n);

    printf("PART 1: Single-threaded odd-even sort\n");
    printf("Unsorted sample: ");
    printArraySample(arr, n, 10);

    start = omp_get_wtime();
    oddEvenSortSingle(arr, n);
    end = omp_get_wtime();

    printf("Sorted sample:   ");
    printArraySample(arr, n, 10);
    printf("Sorted correctly? %s\n", isSorted(arr, n) ? "Yes" : "No");
    printf("Execution time: %.6f seconds\n", end - start);

    free(arr);
    return 0;
}
