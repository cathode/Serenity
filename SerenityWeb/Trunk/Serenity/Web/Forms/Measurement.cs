/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    public struct Measurement
    {
        #region Constructors - Public
        public Measurement(double value, MeasurementUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }
        #endregion
        #region Fields - Private
        private double value;
        private MeasurementUnit unit;
        #endregion
        #region Methods - Public
        public Measurement ConvertUnit(MeasurementUnit newUnit)
        {
            if (this.Unit == newUnit)
            {
                return this;
            }
            else if (!this.IsConvertibleTo(newUnit))
            {
                throw new InvalidOperationException("The current Measurement cannot be converted to the specified Unit.");
            }

            throw new NotImplementedException();
        }
        public override bool Equals(object obj)
        {
            return Measurement.Equals(this, (Measurement)obj);
        }
        public static bool Equals(Measurement a, Measurement b)
        {
            return (a.Unit == b.Unit) && (a.Value == b.Value);
        }
        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ (int)this.Unit;
        }
        public bool IsConvertibleTo(MeasurementUnit newUnit)
        {
            if (this.Unit == newUnit)
            {
                return true;
            }

            switch (this.Unit)
            {
                case MeasurementUnit.Auto:
                    return false;

                case MeasurementUnit.Centimeter:
                case MeasurementUnit.Inch:
                case MeasurementUnit.Milimeter:
                case MeasurementUnit.Pica:
                case MeasurementUnit.Point:
                    switch (newUnit)
                    {
                        case MeasurementUnit.Centimeter:
                        case MeasurementUnit.Inch:
                        case MeasurementUnit.Milimeter:
                        case MeasurementUnit.Pica:
                        case MeasurementUnit.Point:
                            return true;
                    }
                    break;

                case MeasurementUnit.FontHeightRelative:
                    return false;

                case MeasurementUnit.FontRelative:
                    return false;

                case MeasurementUnit.Percent:
                    return false;

                case MeasurementUnit.Pixel:
                    return false;
            }
            return false;
        }
        public override string ToString()
        {
            switch (this.Unit)
            {
                case MeasurementUnit.Auto:
                    return "auto";
                case MeasurementUnit.Centimeter:
                    return this.Value.ToString() + "cm";
                case MeasurementUnit.FontHeightRelative:
                    return this.Value.ToString() + "em";
                case MeasurementUnit.FontRelative:
                    return this.Value.ToString() + "ex";
                case MeasurementUnit.Inch:
                    return this.Value.ToString() + "in";
                case MeasurementUnit.Milimeter:
                    return this.Value.ToString() + "mm";
                case MeasurementUnit.Percent:
                    return this.Value.ToString() + "%";
                case MeasurementUnit.Pica:
                    return this.Value.ToString() + "pc";
                case MeasurementUnit.Pixel:
                    return ((int)this.Value).ToString() + "px";
                case MeasurementUnit.Point:
                    return this.Value.ToString() + "pt";

                default:
                    return this.Value.ToString();
            }
        }
        #endregion
        #region Operators
        public static bool operator ==(Measurement a, Measurement b)
        {
            return Measurement.Equals(a, b);
        }
        public static bool operator !=(Measurement a, Measurement b)
        {
            return !Measurement.Equals(a, b);
        }
        public static Measurement operator +(Measurement a, Measurement b)
        {
            if (a.Unit != b.unit && !a.IsConvertibleTo(b.Unit))
            {
                throw new ArgumentException("Cannot operate on Measurement instances when they have incompatible MeasurementUnit values.");
            }
            return new Measurement(a.Value + b.Value, a.Unit);
        }
        public static Measurement operator -(Measurement a, Measurement b)
        {
            if (a.Unit != b.Unit)
            {
                if (!a.IsConvertibleTo(b.Unit))
                {
                    throw new ArgumentException("Cannot operate on Measurement instances when they have incompatible MeasurementUnit values.");
                }
            }

            throw new NotImplementedException();
        }
        #endregion
        #region Properties - Public
        public double Value
        {
            get
            {
                return this.value;
            }
        }
        public MeasurementUnit Unit
        {
            get
            {
                return this.unit;
            }
        }
        #endregion
    }
}
