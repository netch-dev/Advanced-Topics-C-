public class Numerics {
	/////////////////////////////
	// Integral types and limits
	/////////////////////////////

	// 8 bit
	private sbyte sbyte_min = -128;
	private sbyte sbyte_max = 127;

	// 16 bit
	private short short_min = -32768;
	private short short_max = 32767;

	// 32 bit
	private int int_min = -2147483648;
	private int int_max = 2147483647;

	// 64 bit
	private long long_min = -9223372036854775808;
	private long long_max = 9223372036854775807;

	// BigInteger is an integer or arbitrary size. Good for working with large numbers
	// - Is a struct
	// - Cannot overflow
	// - Is immutable like strings, so operations return a new instance

	/////////////////////////////
	// Floating Point Values
	/////////////////////////////

	// - FP calculations do not cause exceptions

	// - Division by zero is allowed and returns Infinity
	// -- There is no way of moving from infinity back to a measureable number

	// - NaN is returned when you divide 0 by 0 or infinity by infinity
	// -- It has no sign
	// -- It is viral (any operation on NaN yields a NaN
	// -- Comparisons with NaN always return false
	// -- To check for NaN, use float.IsNaN() or double.IsNaN()

	// - FP values are not precise, some numbers are not exactly what you intend them to be
	// -- This can lead to surprising results:
	// --- double d = 0.1 + 0.2 = 0.30000000000000004
	// --- (0.1 + 0.2 == 0.3) // Returns false

	// -- Comparing FP values should be done with a tolerance
	// --- Math.Abs((a - b) / a) < tolerance
	// --- In real world code, tolerance values are hard to find

	// 32 bit
	private float float_min = -3.40282347E+38F;
	private float float_max = 3.40282347E+38F;

	// 64 bit
	private double double_min = -1.7976931348623157E+308;
	private double double_max = 1.7976931348623157E+308;

	/////////////////////////////
	// System.Decimal
	/////////////////////////////

	// - Used for financial calculations
	// - Not subject to FP representation errors
	// -- 0.1m + 0.2m == 0.3m

	// Slower than FP calculations

	// 128 bit
	private decimal decimal_min = -79228162514264337593543950335M;
	private decimal decimal_max = 79228162514264337593543950335M;

	/////////////////////////////
	// Vector Types
	/////////////////////////////

	// - The CPU has registers larger than 64 bits
	// -- 128, 256, 512 bits depending on the CPU

	// - Those registers are not for data types with wider range
	// - Instead they are used for packed values
	// -- A 128-bit register can pack:
	// --- 16 bytes
	// --- 4 int values
	// --- 4 float values
	// --- 2 double values

	// - Motivation to use vector types:
	// - Processor instructions have a cost
	// - Add < multiply < divide < square root

	// - SIMD lets us accelerate these operations, and we need vector types to use SIMD
	// -- Single Instruction, Multiple Data
	// -- Primarily used for FP calculations

	// -- SIMD-accelerated types (float based): Matrix3x2, Matrix4x4, Plane, Quaternion, Vector2, Vector3, Vector4
	// --- Also general purpose types: Vector<T>, where T is a numeric type and the size is not statically known
	// --- Vector<T> allows you to write generalized code that can work on different vector sizes
	// -- You can combine SIMD and multithreading
}